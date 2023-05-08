using MeShopAlot.Data.Model.Base;

namespace MeShopAlot.Data.Models
{
    public class DeliveryMethod : BasePersistentObject
    {
        #region Physical Properties

        public string DeliveryTime { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ShortName { get; set; }

        #endregion Physical Properties
    }
}
