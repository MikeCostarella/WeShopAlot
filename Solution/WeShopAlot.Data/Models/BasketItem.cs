using WeShopAlot.Data.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeShopAlot.Data.Models
{
    public class BasketItem : BasePersistentObject
    {
        #region Physical Properties

        public string Brand { get; set; }

        public string PictureUrl { get; set; }

        [Column(TypeName = "decimal (5,2)")]
        public decimal Price { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public string Type { get; set; }

        #endregion Physical Properties
    }
}
