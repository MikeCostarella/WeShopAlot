using MeShopAlot.Data.Seed.Common;
using MeShopAlot.Shared.Extensions;

namespace MeShopAlot.Data.Seed
{
    public class EnumSeedingExtension
    {
        public static List<TResult> SeedEnumValues<TEnum, TResult>() where TEnum : System.Enum
        {
            var enumInfos = Enum.GetValues(typeof(TEnum)).Cast<int>()
                .Zip(Enum.GetNames(typeof(TEnum)), (i, s) => (Value: i, Name: s));
            var modelObject = typeof(TResult);
            var modelObjectProertyId = modelObject.GetProperty("Id");
            var modelObjectPropertyName = modelObject.GetProperty("Name");
            var modelObjectPropertyDescription = modelObject.GetProperty("Description");
            return enumInfos.Select(i =>
            {
                var instance = (TResult)Activator.CreateInstance(modelObject);
                modelObjectProertyId.SetValue(instance, i.Value);
                modelObjectPropertyName.SetValue(instance, EnumX.GetValue<TEnum>(i.Name));
                modelObjectPropertyDescription.SetValue(instance, EnumX.GetValue<TEnum>(i.Name).GetDescription());
                return instance;
            }
            ).ToList();
        }
    }
}
