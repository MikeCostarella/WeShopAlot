namespace WeShopAlot.Data.Import.Models
{
    public class ImportedProduct
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int ProductBrandId { get; set; }
        public int ProductTypeId { get; set; }
    }
}
