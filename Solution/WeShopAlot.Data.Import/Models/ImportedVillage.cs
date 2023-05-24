namespace WeShopAlot.Data.Import.Models
{
    public class ImportedVillage
    {
        public string CountyName { get; set; }
        public string VillageName { get; set; }
        public string FormOfGovernment { get; set; }
        public int YearIncorporated { get; set; }
        public string Census2000 { get; set; }
        public string Census2010 { get; set; }
        public string Census2020 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int? ZIPCode { get; set; }
        public string Telephone { get; set; }
        public string Website { get; set; }
    }
}
