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

        public string Manager { get; set; }
        public string ManagerFirstName { get; set; }
        public string ManagerMiddleName { get; set; }
        public string ManagerLastName { get; set; }
        public string ManagerTermEndDate { get; set; }

        public string Administrator { get; set; }
        public string AdministratorFirstName { get; set; }
        public string AdministratorMiddleName { get; set; }
        public string AdministratorLastName { get; set; }
        public string AdministratorTermEndDate { get; set; }

        public string Auditor { get; set; }
        public string AuditorFirstName { get; set; }
        public string AuditorMiddleName { get; set; }
        public string AuditorLastName { get; set; }
        public string AuditorTermEndDate { get; set; }

        public string Treasurer { get; set; }
        public string TreasurerFirstName { get; set; }
        public string TreasurerMiddleName { get; set; }
        public string TreasurerLastName { get; set; }
        public string TreasurerTermEndDate { get; set; }

        public string LawDirector { get; set; }
        public string LawDirectorFirstName { get; set; }
        public string LawDirectorMiddleName { get; set; }
        public string LawDirectorLastName { get; set; }
        public string LawDirectorTermEndDate { get; set; }

        public string CouncilPresident { get; set; }
        public string CouncilPresidentFirstName { get; set; }
        public string CouncilPresidentMiddleName { get; set; }
        public string CouncilPresidentLastName { get; set; }
        public string CouncilPresidentTermEndDate { get; set; }
    }
}
