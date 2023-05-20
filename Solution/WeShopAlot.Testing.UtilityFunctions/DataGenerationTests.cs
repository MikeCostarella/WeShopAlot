using System.Text.Json;
using WeShopAlot.Data.Import.Models;
using WeShopAlot.Data.Shared.Enumerations.OhioSpecific;
using WeShopAlot.Shared.Extensions;

namespace WeShopAlot.Testing.UtilityFunctions
{
    [TestClass]
    public class DataGenerationTests
    {
        [TestMethod]
        public void GenerateOhioTownshipJson()
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            var enumValues = Enum.GetValues(typeof(OhioTownshipEnum));
            var townships = new List<ImportedTownship>();
            foreach (var enumValue in enumValues)
            {
                var s = enumValue.GetDescription();
                var countyName = s.Split('-').First().Trim();
                var townshipName = s.Split('-')[1].Trim();
                var townshipObject = new ImportedTownship
                {
                    County = countyName,
                    Name = townshipName
                };
                townships.Add(townshipObject);
            }
            var jsonString = System.Text.Json.JsonSerializer.Serialize(townships, options);
        }
    }

}