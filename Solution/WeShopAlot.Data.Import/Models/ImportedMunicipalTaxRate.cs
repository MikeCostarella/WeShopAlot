namespace WeShopAlot.Data.Import.Models
{
    public class ImportedMunicipalTaxRate
    {
        public int StartDate { get; set; }
        public int EndDate { get; set; }
        public int MunicipalID { get; set; }
        public string MunicipalityName { get; set; }
        public decimal TaxRate { get; set; }
    }
}
