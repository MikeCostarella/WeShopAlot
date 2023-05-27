using System.Windows;
using WeShopAlot.Data;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories;
using WeShopAlot.Data.Repositories.Interfaces;

namespace WeShopAlot.UI.ClientWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WeShopAlotContext dbContext;
        CountyRepository countyRepository;
        CountryRepository countryRepository;
        Country country;
        MunicipalityRepository municipalityRepository;
        StateProvinceRepository stateProvinceRepository;
        StateProvince stateProvince;
        TownshipRepository townshipRepository;
        public MainWindow()
        {
            InitializeComponent();
            dbContext = WeShopAlotContext.NewFromConnectionString("Server=localhost;Database=WeShopAlot;User Id=WeShopAlot; Password=WeShopAlot;Trusted_Connection=true;TrustServerCertificate=true;");
            countryRepository = new CountryRepository(dbContext);
            country = countryRepository.GetByAbbreviation("US");
            municipalityRepository = new MunicipalityRepository(dbContext);
            stateProvinceRepository = new StateProvinceRepository(dbContext);
            stateProvince = stateProvinceRepository.Get(country, "Ohio");
            countyRepository = new CountyRepository(dbContext);
            townshipRepository = new TownshipRepository(dbContext);
        }

        private void tcOhioDataLake_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch (tcOhioDataLake.SelectedIndex)
            {
                case 0: // Counties
                    dgCounties.ItemsSource = countyRepository.GetAll(stateProvince);
                    break;
                case 1: // Municipalities
                    dgMunicipalities.ItemsSource = municipalityRepository.GetAll(stateProvince);
                    break;
                case 2: // Townships
                    dgTownships.ItemsSource = townshipRepository.GetAll(stateProvince);
                    break;
                case 3:
                    break;
            }
        }
    }
}
