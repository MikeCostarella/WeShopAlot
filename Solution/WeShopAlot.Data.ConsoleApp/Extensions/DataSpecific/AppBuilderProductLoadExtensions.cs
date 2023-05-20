using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using WeShopAlot.Data.Import.Models;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories;

namespace WeShopAlot.Data.ConsoleApp.Extensions.DataSpecific
{
    public static class AppBuilderProductLoadExtensions
    {
        public static void LoadProducts(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = servicedScope.ServiceProvider.GetRequiredService<WeShopAlotContext>();
                var productBrandRepository = new ProductBrandRepository(context);
                var productTypeRepository = new ProductTypeRepository(context);
                try
                {
                    var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    if (!context.Products.Any())
                    {
                        var productsData = File.ReadAllText(path + @"/Content/data/json/products.json");
                        var importedProducts = JsonSerializer.Deserialize<List<ImportedProduct>>(productsData);
                        foreach (var importedProduct in importedProducts)
                        {
                            var productBrand = productBrandRepository.GetByName(importedProduct.ProductBrand);
                            var productType = productTypeRepository.GetByName(importedProduct.ProductType);
                            var product = new Product
                            {
                                Description = importedProduct.Description,
                                InternalId = importedProduct.InternalId,
                                Name = importedProduct.Name,
                                PictureUrl = importedProduct.PictureUrl,
                                Price = importedProduct.Price,
                                ProductBrandId = productBrand.Id,
                                ProductTypeId = productType.Id
                            };
                            context.Products.Add(product);
                        }
                    }
                    if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
