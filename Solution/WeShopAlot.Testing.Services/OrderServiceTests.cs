using WeShopAlot.Data.Shared.Enumerations;
using WeShopAlot.Infrastructure.Services;

namespace WeShopAlot.Testing.Services
{
    /// <summary>
    /// OrderService with its real UnitOfWork and GenericRepository on an in-memory database, and
    /// the basket in a dictionary. No controller, no HTTP, no JWT: the test calls the service the
    /// way OrdersController does and then reads the database back through a fresh context.
    ///
    /// Compare ApiTests.Checkout_BasketToOrder_UsesServerPrices, which checks the same rule through
    /// POST /api/order. That test proves the route, the token, and the DTO mapping as well; this
    /// one isolates the service, runs in milliseconds, and fails with a stack trace that points at
    /// OrderService rather than at a 500 response.
    /// </summary>
    [TestClass]
    public class OrderServiceTests
    {
        private const string Buyer = "ada@example.com";

        private ServiceTestDatabase database;
        private InMemoryBasketRepository baskets;
        private OrderService service;

        private static Address ShipTo() => new Address
        {
            FirstName = "Ada",
            LastName = "Lovelace",
            AddressLine1 = "1 Analytical Way",
            AddressLine2 = "Suite 2",
            City = "Girard",
            State = "OH",
            ZipCode = "44420",
        };

        [TestInitialize]
        public async Task TestInitialize()
        {
            database = await ServiceTestDatabase.CreateSeededAsync();
            baskets = new InMemoryBasketRepository();
            service = new OrderService(baskets, new UnitOfWork(database.CreateContext()));
        }

        /// <summary>
        /// Puts the named catalog products in a basket at a deliberately wrong price, so a test can
        /// tell whether the order used the basket's price or the database's.
        /// </summary>
        private async Task<CustomerBasket> BasketOf(string basketId, string paymentIntentId, params (string Name, int Quantity)[] lines)
        {
            await using var db = database.CreateContext();
            var basket = new CustomerBasket { Id = basketId, PaymentIntentId = paymentIntentId };
            foreach (var (name, quantity) in lines)
            {
                var product = await db.Products.SingleAsync(p => p.Name == name);
                basket.Items.Add(new BasketItem { Id = product.Id, ProductName = name, Quantity = quantity, Price = 0.01m });
            }
            return await baskets.UpdateBasketAsync(basket);
        }

        [TestMethod]
        public async Task CreateOrderAsync_UsesTheCatalogPrices_NotTheBaskets()
        {
            // products.json: Blondie 500.00, Dawggy 150.00. The basket claims one cent each.
            await BasketOf("basket-1", "pi_test_1", ("Blondie", 2), ("Dawggy", 1));

            var order = await service.CreateOrderAsync(Buyer, deliveryMethodId: 1, "basket-1", ShipTo());

            Assert.IsNotNull(order);
            Assert.AreEqual(2 * 500m + 150m, order.Subtotal);
            CollectionAssert.AreEquivalent(new[] { 500m, 150m }, order.OrderItems.Select(i => i.Price).ToArray());
            CollectionAssert.AreEquivalent(new[] { "Blondie", "Dawggy" }, order.OrderItems.Select(i => i.ItemOrdered.ProductName).ToArray());
        }

        [TestMethod]
        public async Task CreateOrderAsync_SavesTheOrder_WithItemsAddressAndPendingStatus()
        {
            await BasketOf("basket-1", "pi_test_1", ("Stingray", 1));

            var created = await service.CreateOrderAsync(Buyer, deliveryMethodId: 2, "basket-1", ShipTo());

            // A fresh context: what is in the database, not what the service's own tracker holds.
            await using var db = database.CreateContext();
            var saved = await db.Orders
                .Include(o => o.OrderItems).ThenInclude(i => i.ItemOrdered)
                .Include(o => o.ShipToAddress)
                .Include(o => o.DeliveryMethod)
                .SingleAsync();

            Assert.AreEqual(created.Id, saved.Id);
            Assert.AreEqual(Buyer, saved.BuyerEmail);
            Assert.AreEqual("pi_test_1", saved.PaymentIntentId);
            Assert.AreEqual((int)OrderStatusEnum.Pending, saved.StatusId);
            Assert.AreEqual("UPS2", saved.DeliveryMethod.ShortName);
            Assert.AreEqual("Girard", saved.ShipToAddress.City);
            Assert.AreEqual(1, saved.OrderItems.Count);
            Assert.AreEqual("Stingray", saved.OrderItems[0].ItemOrdered.ProductName);
            Assert.AreEqual(180m, saved.Subtotal);
        }

        [TestMethod]
        public async Task CreateOrderAsync_SamePaymentIntentTwice_UpdatesTheOrderInsteadOfAddingOne()
        {
            // Stripe can deliver a webhook and the client can POST /api/order for the same payment;
            // OrderByPaymentIntentIdSpecification is what keeps that from creating two orders.
            await BasketOf("basket-1", "pi_test_1", ("Blondie", 1));

            var first = await service.CreateOrderAsync(Buyer, deliveryMethodId: 1, "basket-1", ShipTo());
            var second = await service.CreateOrderAsync(Buyer, deliveryMethodId: 3, "basket-1", ShipTo());

            Assert.AreEqual(first.Id, second.Id);

            await using var db = database.CreateContext();
            Assert.AreEqual(1, await db.Orders.CountAsync());
            Assert.AreEqual(3, (await db.Orders.SingleAsync()).DeliveryMethodId);
        }

        [TestMethod]
        public async Task GetOrdersForUserAsync_ReturnsOnlyThatBuyersOrders_NewestFirst()
        {
            await BasketOf("ada-1", "pi_ada_1", ("Blondie", 1));
            await BasketOf("ada-2", "pi_ada_2", ("Dawggy", 1));
            await BasketOf("bob-1", "pi_bob_1", ("Stingray", 1));

            var older = await service.CreateOrderAsync(Buyer, 1, "ada-1", ShipTo());
            var newer = await service.CreateOrderAsync(Buyer, 1, "ada-2", ShipTo());
            await service.CreateOrderAsync("bob@example.com", 1, "bob-1", ShipTo());

            // OrderDate defaults to UtcNow at construction; make the two unmistakably ordered.
            await using (var db = database.CreateContext())
            {
                (await db.Orders.SingleAsync(o => o.Id == older.Id)).OrderDate = DateTime.UtcNow.AddDays(-1);
                await db.SaveChangesAsync();
            }

            var orders = await service.GetOrdersForUserAsync(Buyer);

            Assert.AreEqual(2, orders.Count);
            Assert.IsTrue(orders.All(o => o.BuyerEmail == Buyer));
            Assert.AreEqual(newer.Id, orders[0].Id, "newest order first");
            Assert.IsNotNull(orders[0].OrderItems[0].ItemOrdered, "the specification includes OrderItems.ItemOrdered");
            Assert.IsNotNull(orders[0].DeliveryMethod, "the specification includes DeliveryMethod");
        }

        [TestMethod]
        public async Task GetOrderByIdAsync_AnotherBuyersOrder_ReturnsNull()
        {
            await BasketOf("bob-1", "pi_bob_1", ("Stingray", 1));
            var bobs = await service.CreateOrderAsync("bob@example.com", 1, "bob-1", ShipTo());

            Assert.IsNull(await service.GetOrderByIdAsync(bobs.Id, Buyer));
            Assert.IsNotNull(await service.GetOrderByIdAsync(bobs.Id, "bob@example.com"));
        }

        [TestMethod]
        public async Task GetDeliveryMethodsAsync_ReturnsTheShippedFour()
        {
            var methods = await service.GetDeliveryMethodsAsync();

            Assert.AreEqual(4, methods.Count);
            CollectionAssert.AreEquivalent(new[] { "UPS1", "UPS2", "UPS3", "FREE" }, methods.Select(m => m.ShortName).ToArray());
        }
    }
}
