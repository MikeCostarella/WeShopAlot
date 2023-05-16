using WeShopAlot.Data.Seed.Common;
using WeShopAlot.Shared.Extensions;

namespace WeShopAlot.Data.Seed
{
    public class EnumSeedingExtension
    {
        public static List<TResult> SeedEnumValues<TEnum, TResult>() where TEnum : System.Enum
        {
            var typeOfEnum = typeof(TEnum);
            var enumValues = Enum.GetValues(typeOfEnum);
            var enumValuesCastInt = enumValues.Cast<int>();
            var enumNames = Enum.GetNames(typeof(TEnum));
            var enumInfos = enumValuesCastInt.Zip(enumNames, (i, s) => (Value: i, Name: s));
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
