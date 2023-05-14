using MeShopAlot.Data.Models;
using MeShopAlot.Data.Specifications.Base;

namespace MeShopAlot.Data.Specifications
{
    public class OrderByPaymentIntentIdSpecification : BaseSpecification<Order>
    {
        public OrderByPaymentIntentIdSpecification(string paymentIntentId)
            : base(o => o.PaymentIntentId == paymentIntentId)
        {
        }
    }
}
