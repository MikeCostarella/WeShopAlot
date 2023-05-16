using WeShopAlot.Data.Models;
using WeShopAlot.Data.Specifications.Base;

namespace WeShopAlot.Data.Specifications
{
    public class OrderByPaymentIntentIdSpecification : BaseSpecification<Order>
    {
        public OrderByPaymentIntentIdSpecification(string paymentIntentId)
            : base(o => o.PaymentIntentId == paymentIntentId)
        {
        }
    }
}
