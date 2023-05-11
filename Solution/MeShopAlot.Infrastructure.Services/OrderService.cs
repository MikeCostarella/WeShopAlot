using MeShopAlot.Data.Models;
using MeShopAlot.Data.Repositories.Interfaces;
using MeShopAlot.Data.Specifications;
using MeShopAlot.Infrastructure.Services.Interfaces;

namespace MeShopAlot.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        #region Member Variables

        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;

        #endregion Member Variables

        #region Constructors

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.basketRepository = basketRepository;
        }

        #endregion Constructors

        #region Public Methods

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            // get basket from repo
            var basket = await basketRepository.GetBasketAsync(basketId);

            // get items from the product repo
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered
                {
                    ProductItemId = productItem.Id,
                    ProductName = productItem.Name,
                    PictureUrl = productItem.PictureUrl
                };
                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };
                items.Add(orderItem);
            }

            // get delivery method from repo
            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // calc subtotal
            var subtotal = items.Sum(item => item.Price * item.Quantity);

            // check to see if order exists
            var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
            var order = await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if (order != null)
            {
                order.ShipToAddress = shippingAddress;
                order.DeliveryMethod = deliveryMethod;
                order.Subtotal = subtotal;
                unitOfWork.Repository<Order>().Update(order);
            }
            else
            {
                // create order
                order = new Order
                {
                    OrderItems = items,
                    BuyerEmail = buyerEmail,
                    ShipToAddress = shippingAddress,
                    DeliveryMethod = deliveryMethod,
                    Subtotal = subtotal,
                    PaymentIntentId = basket.PaymentIntentId
                };
                unitOfWork.Repository<Order>().Add(order);
            }

            // save to db
            var result = await unitOfWork.Complete();

            if (result <= 0) return null;

            // return order
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);

            return await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);

            return await unitOfWork.Repository<Order>().ListAsync(spec);
        }

        #endregion Public Methods
    }
}
