using WeShopAlot.Data.Models.Base;

namespace WeShopAlot.Data.Models
{
    public class ProductItemOrdered : BasePersistentObject
    {
        public int ProductItemId { get; set; }

        public string ProductName { get; set; }

        public string PictureUrl { get; set; }
    }
}
