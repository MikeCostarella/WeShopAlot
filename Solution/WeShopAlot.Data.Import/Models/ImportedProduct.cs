namespace WeShopAlot.Data.Import.Models
{
    public class ImportedProduct
    {
        public string Description { get; set; }
        public int InternalId { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public string ProductBrand { get; set; }
        public string ProductType { get; set; }
    }
}
