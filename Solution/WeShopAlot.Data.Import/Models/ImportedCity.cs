namespace WeShopAlot.Data.Import.Models
{
    public class ImportedCity
    {
        public string CountyName { get; set; }
        public string CityName { get; set; }
        public string FormOfGovernment { get; set; }
        public int YearIncorporated { get; set; }
        public int Census2000 { get; set; }
        public int Census2010 { get; set; }
        public int Census2020 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int ZIPCode { get; set; }
        public string Telephone { get; set; }
        public string Website { get; set; }
        public string Mayor { get; set; }
        public string MayorFirstName { get; set; }
        public string MayorMiddleName { get; set; }
        public string MayorLastName { get; set; }
        public string MayorTermEndDate { get; set; }
    }
}
