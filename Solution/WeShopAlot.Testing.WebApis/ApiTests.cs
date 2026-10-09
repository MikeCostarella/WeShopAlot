using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Shared.Enumerations;
using WeShopAlot.Shared.Dtos;

namespace WeShopAlot.Testing.WebApis
{
    /// <summary>
    /// End-to-end checks of the API contract: status codes, response shapes,
    /// and the sign-in flow, sent as real HTTP requests to the in-memory app.
    /// </summary>
    [TestClass]
    public class ApiTests
    {
        private static WeShopAlotApiFactory factory;
        private static HttpClient client;
        private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

        [ClassInitialize]
        public static async Task ClassInitialize(TestContext context)
        {
            factory = new WeShopAlotApiFactory();
            await factory.SeedAsync();
            client = factory.CreateClient();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            client?.Dispose();
            factory?.Dispose();
        }

        // ------------------------------------------------------------ catalog

        [TestMethod]
        public async Task Products_ReturnsPagingEnvelope()
        {
            var response = await client.GetAsync("/api/product?pageSize=2&sort=priceAsc");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var page = await ReadAsync(response);
            Assert.AreEqual(1, page.GetProperty("pageIndex").GetInt32());
            Assert.AreEqual(2, page.GetProperty("pageSize").GetInt32());
            Assert.AreEqual(3, page.GetProperty("count").GetInt32());
            var data = page.GetProperty("data");
            Assert.AreEqual(2, data.GetArrayLength());
            Assert.AreEqual("Core Purple Hat", data[0].GetProperty("name").GetString(), "priceAsc puts the cheapest first");
        }

        [TestMethod]
        public async Task Products_CapsPageSizeAt50()
        {
            var page = await ReadAsync(await client.GetAsync("/api/product?pageSize=500"));
            Assert.AreEqual(50, page.GetProperty("pageSize").GetInt32());
        }

        [TestMethod]
        public async Task Products_FiltersByBrandAndType()
        {
            var page = await ReadAsync(await client.GetAsync("/api/product?brandId=1&typeId=1"));
            Assert.AreEqual(2, page.GetProperty("count").GetInt32());
        }

        [TestMethod]
        public async Task Products_SearchesByName()
        {
            var page = await ReadAsync(await client.GetAsync("/api/product?search=hat"));
            Assert.AreEqual(1, page.GetProperty("count").GetInt32());
        }

        [TestMethod]
        public async Task Product_MapsBrandTypeAndFullPictureUrl()
        {
            var product = await client.GetFromJsonAsync<ProductToReturnDto>("/api/product/3", Json);
            Assert.AreEqual("NetCore", product.ProductBrand);
            Assert.AreEqual("Hats", product.ProductType);
            StringAssert.EndsWith(product.PictureUrl, "images/products/hat-core1.png");
        }

        [TestMethod]
        public async Task Product_Unknown_Returns404WithErrorBody()
        {
            var response = await client.GetAsync("/api/product/999");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.AreEqual(404, (await ReadAsync(response)).GetProperty("statusCode").GetInt32());
        }

        [TestMethod]
        public async Task Brands_And_Types_ReturnLookupLists()
        {
            Assert.AreEqual(2, (await ReadAsync(await client.GetAsync("/api/product/brands"))).GetArrayLength());
            Assert.AreEqual(2, (await ReadAsync(await client.GetAsync("/api/product/types"))).GetArrayLength());
        }

        // ------------------------------------------------------------- errors

        [TestMethod]
        public async Task Buggy_EndpointsReturnTheirStatusCodes()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, (await client.GetAsync("/api/buggy/badrequest")).StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, (await client.GetAsync("/api/buggy/notfound")).StatusCode);
            Assert.AreEqual(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/buggy/testauth")).StatusCode);

            var serverError = await client.GetAsync("/api/buggy/servererror");
            Assert.AreEqual(HttpStatusCode.InternalServerError, serverError.StatusCode);
            Assert.AreEqual(500, (await ReadAsync(serverError)).GetProperty("statusCode").GetInt32(), "the exception middleware returns JSON");
        }

        [TestMethod]
        public async Task Register_InvalidPassword_Returns400WithErrors()
        {
            var response = await client.PostAsJsonAsync("/api/account/register",
                new RegisterDto { DisplayName = "Weak", Email = "weak@test.com", Password = "password" });
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsTrue((await ReadAsync(response)).GetProperty("errors").GetArrayLength() > 0);
        }

        // ----------------------------------------------------- authentication

        [TestMethod]
        public async Task Orders_WithoutToken_Returns401()
        {
            Assert.AreEqual(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/order")).StatusCode);
        }

        [TestMethod]
        public async Task Login_WrongPassword_Returns401()
        {
            await RegisterAsync("wrongpw@test.com");
            var response = await client.PostAsJsonAsync("/api/account/login", new LoginDto { Email = "wrongpw@test.com", Password = "Not-The-Password1" });
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Register_DuplicateEmail_Returns400()
        {
            await RegisterAsync("dupe@test.com");
            var again = await client.PostAsJsonAsync("/api/account/register",
                new RegisterDto { DisplayName = "Dupe", Email = "dupe@test.com", Password = "Pa$$w0rd" });
            Assert.AreEqual(HttpStatusCode.BadRequest, again.StatusCode);
        }

        [TestMethod]
        public async Task Token_FromLogin_IsAcceptedByProtectedEndpoints()
        {
            await RegisterAsync("buyer@test.com");
            var login = await client.PostAsJsonAsync("/api/account/login", new LoginDto { Email = "buyer@test.com", Password = "Pa$$w0rd" });
            Assert.AreEqual(HttpStatusCode.OK, login.StatusCode);
            var user = await login.Content.ReadFromJsonAsync<UserDto>(Json);

            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/order");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            var orders = await client.SendAsync(request);
            Assert.AreEqual(HttpStatusCode.OK, orders.StatusCode, "a token issued by login must pass the API issuer check");
        }

        [TestMethod]
        public async Task Address_NoneSavedYet_Returns204()
        {
            var token = await RegisterAsync("noaddress@test.com");
            using var get = new HttpRequestMessage(HttpMethod.Get, "/api/account/address");
            get.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Assert.AreEqual(HttpStatusCode.NoContent, (await client.SendAsync(get)).StatusCode,
                "checkout asks for the saved address on every visit; a new user has none");
        }

        [TestMethod]
        public async Task Address_FullStateName_Returns400NotA500()
        {
            var token = await RegisterAsync("ohio@test.com");
            var address = new AddressDto { FirstName = "Ada", LastName = "Lovelace", Street = "1 Main St", City = "Girard", State = "Ohio", Zipcode = "44420" };
            using var put = new HttpRequestMessage(HttpMethod.Put, "/api/account/address") { Content = JsonContent.Create(address) };
            put.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.SendAsync(put);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "the database column holds 2 characters");
            StringAssert.Contains(await response.Content.ReadAsStringAsync(), "2-letter");
        }

        [TestMethod]
        public async Task Address_PutThenGet_RoundTrips()
        {
            var token = await RegisterAsync("address@test.com");
            var address = new AddressDto { FirstName = "Ada", LastName = "Lovelace", Street = "1 Main St", City = "Girard", State = "OH", Zipcode = "44420" };

            using var put = new HttpRequestMessage(HttpMethod.Put, "/api/account/address") { Content = JsonContent.Create(address) };
            put.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Assert.AreEqual(HttpStatusCode.OK, (await client.SendAsync(put)).StatusCode);

            using var get = new HttpRequestMessage(HttpMethod.Get, "/api/account/address");
            get.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var saved = await (await client.SendAsync(get)).Content.ReadFromJsonAsync<AddressDto>(Json);
            Assert.AreEqual("1 Main St", saved.Street);
            Assert.AreEqual("44420", saved.Zipcode);
            Assert.AreEqual("Ada", saved.FirstName, "the name used to be dropped because Address had no name columns");
            Assert.AreEqual("Lovelace", saved.LastName);
        }

        // ------------------------------------------------- basket and checkout

        [TestMethod]
        public async Task Checkout_BasketToOrder_UsesServerPrices()
        {
            var token = await RegisterAsync("checkout@test.com");
            var basket = new CustomerBasketDto
            {
                Id = "basket-" + Guid.NewGuid(),
                Items = new List<BasketItemDto>
                {
                    // The client claims a price of 1; the order must use the catalog price of 200.
                    new() { Id = 1, ProductName = "Angular Speedster Board", Price = 1m, Quantity = 2, PictureUrl = "x", Brand = "Angular", Type = "Boards" }
                }
            };
            Assert.AreEqual(HttpStatusCode.OK, (await client.PostAsJsonAsync("/api/basket", basket)).StatusCode);
            var stored = await client.GetFromJsonAsync<CustomerBasketDto>("/api/basket?id=" + basket.Id, Json);
            Assert.AreEqual(1, stored.Items.Count);

            using var deliveryRequest = new HttpRequestMessage(HttpMethod.Get, "/api/order/deliveryMethods");
            deliveryRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Assert.AreEqual(2, (await ReadAsync(await client.SendAsync(deliveryRequest))).GetArrayLength());

            var orderDto = new OrderDto
            {
                BasketId = basket.Id,
                DeliveryMethodId = 1,
                ShipToAddress = new AddressDto { FirstName = "Ada", LastName = "Lovelace", Street = "1 Main St", City = "Girard", State = "OH", Zipcode = "44420" }
            };
            using var create = new HttpRequestMessage(HttpMethod.Post, "/api/order") { Content = JsonContent.Create(orderDto) };
            create.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var created = await client.SendAsync(create);
            Assert.AreEqual(HttpStatusCode.OK, created.StatusCode, await created.Content.ReadAsStringAsync());
            var createdJson = await ReadAsync(created);
            Assert.AreEqual(400m, createdJson.GetProperty("subtotal").GetDecimal());

            // The success page and My Orders read the order back. That used to be a 500 because the order's
            // items, address and status were not loaded before mapping.
            var orderId = createdJson.GetProperty("id").GetInt32();
            using var getOne = new HttpRequestMessage(HttpMethod.Get, "/api/order/" + orderId);
            getOne.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var oneResponse = await client.SendAsync(getOne);
            Assert.AreEqual(HttpStatusCode.OK, oneResponse.StatusCode, await oneResponse.Content.ReadAsStringAsync());
            var orderJson = await ReadAsync(oneResponse);
            StringAssert.EndsWith(orderJson.GetProperty("orderDate").GetString(), "Z", "the browser must know the date is UTC");
            var order = orderJson.Deserialize<OrderToReturnDto>(Json);
            Assert.AreEqual(1, order.OrderItems.Count);
            Assert.AreEqual("Angular Speedster Board", order.OrderItems[0].ProductName);
            Assert.AreEqual("Ada", order.ShipToAddress.FirstName);
            Assert.AreEqual("Pending", order.Status);
            Assert.AreEqual(order.Subtotal + order.ShippingPrice, order.Total);

            using var getAll = new HttpRequestMessage(HttpMethod.Get, "/api/order");
            getAll.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var allResponse = await client.SendAsync(getAll);
            Assert.AreEqual(HttpStatusCode.OK, allResponse.StatusCode, await allResponse.Content.ReadAsStringAsync());
            Assert.AreEqual(1, (await ReadAsync(allResponse)).GetArrayLength());
        }

        [TestMethod]
        public void NewOrder_StartsPending()
        {
            // The in-memory test database does not enforce foreign keys, so the checkout test above cannot
            // catch a StatusId of 0; SQL Server rejected it because OrderStatus Ids start at 1.
            Assert.AreEqual((int)OrderStatusEnum.Pending, new Order().StatusId);
        }

        [TestMethod]
        public async Task PaymentIntent_UnknownBasket_Returns400()
        {
            // Reaching the action at all proves the payment endpoints resolve their filters (the old
            // [AutoValidateAntiforgeryToken] made every call a 500). An unknown basket stops before Stripe is called.
            var token = await RegisterAsync("payer@test.com");
            using var request = new HttpRequestMessage(HttpMethod.Post, "/api/payment/no-such-basket");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Assert.AreEqual(HttpStatusCode.BadRequest, (await client.SendAsync(request)).StatusCode);
        }

        [TestMethod]
        public async Task Webhook_WithoutValidSignature_Returns400()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "/api/payment/webhook")
            {
                Content = new StringContent("{\"type\":\"payment_intent.succeeded\"}", System.Text.Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Stripe-Signature", "t=1,v1=forged");
            Assert.AreEqual(HttpStatusCode.BadRequest, (await client.SendAsync(request)).StatusCode);
        }

        // ------------------------------------------------------------ helpers

        /// <summary>Registers a user with a valid password and returns the token.</summary>
        private static async Task<string> RegisterAsync(string email)
        {
            var response = await client.PostAsJsonAsync("/api/account/register",
                new RegisterDto { DisplayName = email.Split("@")[0], Email = email, Password = "Pa$$w0rd" });
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, await response.Content.ReadAsStringAsync());
            return (await response.Content.ReadFromJsonAsync<UserDto>(Json)).Token;
        }

        private static async Task<JsonElement> ReadAsync(HttpResponseMessage response) =>
            JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
    }
}
