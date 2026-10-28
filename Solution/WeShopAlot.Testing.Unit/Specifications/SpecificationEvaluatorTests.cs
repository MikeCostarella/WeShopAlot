using WeShopAlot.Data.Models;
using WeShopAlot.Data.Specifications;
using WeShopAlot.Data.Specifications.Base;
using WeShopAlot.Data.Utilities.Specifications;

namespace WeShopAlot.Testing.Unit.Specifications
{
    /// <summary>
    /// The evaluator composes a specification onto any IQueryable. Against a List,
    /// LINQ-to-Objects does the work and EF Core's Include is a no-op, so the whole
    /// filter -> sort -> page pipeline can be proven without a database.
    /// </summary>
    [TestClass]
    public class SpecificationEvaluatorTests
    {
        private static IQueryable<Product> Catalog() => new List<Product>
        {
            new() { Id = 1, Name = "Angular Speedster Board", Price = 200m, ProductBrandId = 1, ProductTypeId = 1 },
            new() { Id = 2, Name = "Core Purple Hat",          Price = 10m,  ProductBrandId = 2, ProductTypeId = 2 },
            new() { Id = 3, Name = "Core Blue Hat",            Price = 15m,  ProductBrandId = 2, ProductTypeId = 2 },
            new() { Id = 4, Name = "React Gloves",             Price = 25m,  ProductBrandId = 3, ProductTypeId = 3 },
            new() { Id = 5, Name = "Angular Boots",            Price = 180m, ProductBrandId = 1, ProductTypeId = 4 },
        }.AsQueryable();

        private static string[] Names(IQueryable<Product> q) => q.Select(p => p.Name).ToArray();

        [TestMethod]
        public void EmptySpecification_ReturnsTheCatalogUntouched()
        {
            var result = SpecificationEvaluator<Product>.GetQuery(Catalog(), new BaseSpecification<Product>());

            Assert.AreEqual(5, result.Count());
        }

        [TestMethod]
        public void PriceAsc_ThenPageTwoOfTwo_ReturnsTheThirdAndFourthCheapest()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(new ProductSpecParams { Sort = "priceAsc", PageSize = 2, PageIndex = 2 });

            var result = SpecificationEvaluator<Product>.GetQuery(Catalog(), spec);

            CollectionAssert.AreEqual(new[] { "React Gloves", "Angular Boots" }, Names(result), "sort happens before Skip/Take");
        }

        [TestMethod]
        public void PriceDesc_PutsTheMostExpensiveFirst()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(new ProductSpecParams { Sort = "priceDesc", PageSize = 50 });

            var result = SpecificationEvaluator<Product>.GetQuery(Catalog(), spec);

            Assert.AreEqual("Angular Speedster Board", Names(result)[0]);
            Assert.AreEqual("Core Purple Hat", Names(result)[^1]);
        }

        [TestMethod]
        public void Search_FiltersBeforeSortingAndPaging()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(new ProductSpecParams { Search = "hat", Sort = "priceAsc", PageSize = 1 });

            var result = SpecificationEvaluator<Product>.GetQuery(Catalog(), spec);

            CollectionAssert.AreEqual(new[] { "Core Purple Hat" }, Names(result), "the cheapest hat, not the cheapest product");
        }

        [TestMethod]
        public void BrandFilter_CountsOnlyThatBrand()
        {
            var spec = new ProductsWithFiltersForCountSpecification(new ProductSpecParams { BrandId = 1 });

            var result = SpecificationEvaluator<Product>.GetQuery(Catalog(), spec);

            Assert.AreEqual(2, result.Count(), "the count specification feeds the paging envelope's total");
        }
    }
}
