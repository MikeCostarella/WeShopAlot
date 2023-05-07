using MeShopAlot.Data.Model.Base;

namespace MeShopAlot.Data.Models
{
    public class BasketItem : BasePersistentObject
    {
        #region Physical Properties

        public int Id { get; set; }

        public string Brand { get; set; }

        public string PictureUrl { get; set; }

        public decimal Price { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public string Type { get; set; }

        #endregion Physical Properties
    }
}
