using WeShopAlot.UI.ClientWPF.Models;

namespace WeShopAlot.UI.ClientWPF.Services
{
    /// <summary>
    /// The shopping basket, like Angular's BasketService: the basket lives in the API (Redis); this class keeps
    /// the current copy, remembers its id between runs, and works out the totals.
    /// </summary>
    public class BasketService
    {
        private readonly ApiClient api;
        private readonly SessionStore session;

        public BasketService(ApiClient api, SessionStore session)
        {
            this.api = api;
            this.session = session;
        }

        /// <summary>Raised whenever the basket or its totals change.</summary>
        public event EventHandler? Changed;

        public CustomerBasket? Basket { get; private set; }

        public BasketTotals? Totals { get; private set; }

        public int ItemCount => Basket?.Items.Sum(i => i.Quantity) ?? 0;

        public int QuantityOf(int productId) => Basket?.Items.FirstOrDefault(i => i.Id == productId)?.Quantity ?? 0;

        /// <summary>At startup: fetch the basket whose id was saved last time.</summary>
        public async Task LoadAsync()
        {
            if (string.IsNullOrEmpty(session.BasketId)) return;
            var basket = await api.GetBasketAsync(session.BasketId);
            if (basket is null || basket.Items.Count == 0) ClearLocal(); // expired or emptied elsewhere
            else Set(basket);
        }

        /// <summary>Adds quantity of a product (or of an existing line) and saves the basket to the API.</summary>
        public async Task AddItemAsync(Product product, int quantity = 1)
        {
            var basket = Basket ?? CreateBasket();
            var item = basket.Items.FirstOrDefault(i => i.Id == product.Id);
            if (item is null)
            {
                basket.Items.Add(new BasketItem
                {
                    Id = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    PictureUrl = product.PictureUrl ?? "",
                    Brand = product.ProductBrand,
                    Type = product.ProductType
                });
            }
            else
            {
                item.Quantity += quantity;
            }
            await SaveAsync(basket);
        }

        public async Task IncrementAsync(int productId)
        {
            var item = Basket?.Items.FirstOrDefault(i => i.Id == productId);
            if (Basket is null || item is null) return;
            item.Quantity++;
            await SaveAsync(Basket);
        }

        /// <summary>Removes quantity of a product; the line goes at 0 and the whole basket goes when it is empty.</summary>
        public async Task RemoveItemAsync(int productId, int quantity = 1)
        {
            var basket = Basket;
            var item = basket?.Items.FirstOrDefault(i => i.Id == productId);
            if (basket is null || item is null) return;

            item.Quantity -= quantity;
            if (item.Quantity <= 0) basket.Items.Remove(item);

            if (basket.Items.Count > 0) await SaveAsync(basket);
            else await DeleteAsync();
        }

        /// <summary>Sets the line to an exact quantity (product details' "Update basket").</summary>
        public async Task SetQuantityAsync(Product product, int quantity)
        {
            var current = QuantityOf(product.Id);
            if (quantity > current) await AddItemAsync(product, quantity - current);
            else if (quantity < current) await RemoveItemAsync(product.Id, current - quantity);
        }

        public async Task SetDeliveryMethodAsync(DeliveryMethod deliveryMethod)
        {
            if (Basket is null) return;
            Basket.DeliveryMethodId = deliveryMethod.Id;
            Basket.ShippingPrice = deliveryMethod.Price;
            await SaveAsync(Basket);
        }

        /// <summary>Asks the API to create (or update) the Stripe payment intent for this basket.</summary>
        public async Task CreatePaymentIntentAsync()
        {
            if (Basket is null) throw new InvalidOperationException("There is no basket.");
            var basket = await api.CreatePaymentIntentAsync(Basket.Id)
                ?? throw new InvalidOperationException("The API did not return the basket.");
            Set(basket);
        }

        public async Task DeleteAsync()
        {
            if (Basket is not null) await api.DeleteBasketAsync(Basket.Id);
            ClearLocal();
        }

        public void ClearLocal()
        {
            Basket = null;
            Totals = null;
            session.BasketId = null;
            session.Save();
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private CustomerBasket CreateBasket()
        {
            var basket = new CustomerBasket();
            session.BasketId = basket.Id;
            session.Save();
            return basket;
        }

        private async Task SaveAsync(CustomerBasket basket)
        {
            var saved = await api.SetBasketAsync(basket) ?? basket;
            Set(saved);
        }

        private void Set(CustomerBasket basket)
        {
            Basket = basket;
            if (session.BasketId != basket.Id)
            {
                session.BasketId = basket.Id;
                session.Save();
            }
            var subtotal = basket.Items.Sum(i => i.Price * i.Quantity);
            Totals = new BasketTotals(basket.ShippingPrice, subtotal, subtotal + basket.ShippingPrice);
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}
