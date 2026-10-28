using WeShopAlot.Data.Models;
using WeShopAlot.Data.Specifications;

namespace WeShopAlot.Testing.Unit.Specifications
{
    /// <summary>
    /// The specification turns ProductSpecParams into a filter, a sort, and a page.
    /// It is a plain object - no DbContext - so each decision it makes is one test.
    /// </summary>
    [TestClass]
    public class ProductsWithTypesAndBrandsSpecificationTests
    {
        // Fixture helpers: a healthy product, then each test overrides one thing.
        private static Product Product(string name, decimal price = 10m, int brandId = 1, int typeId = 1) =>
            new() { Id = 1, Name = name, Price = price, ProductBrandId = brandId, ProductTypeId = typeId };

        private static bool Matches(ProductsWithTypesAndBrandsSpecification spec, Product p) =>
            spec.Criteria.Compile()(p);

        // --------------------------------------------------------------- paging

        [TestMethod]
        public void Paging_FirstPage_SkipsNothing()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(new ProductSpecParams { PageIndex = 1, PageSize = 6 });

            Assert.IsTrue(spec.IsPagingEnabled);
            Assert.AreEqual(0, spec.Skip);
            Assert.AreEqual(6, spec.Take);
        }

        [TestMethod]
        public void Paging_SkipsEveryWholePageBeforeTheRequestedOne()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(new ProductSpecParams { PageIndex = 3, PageSize = 6 });

            Assert.AreEqual(12, spec.Skip, "page 3 of 6 starts after 2 full pages");
            Assert.AreEqual(6, spec.Take);
        }

        // ---------------------------------------------------------------- sorting

        [TestMethod]
        public void Sort_Default_IsByName()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(new ProductSpecParams());

            Assert.AreEqual("Zebra Board", (string)spec.OrderBy.Compile()(Product("Zebra Board")));
            Assert.IsNull(spec.OrderByDescending);
        }

        [TestMethod]
        public void Sort_PriceAsc_OrdersByPriceAscending()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(new ProductSpecParams { Sort = "priceAsc" });

            Assert.AreEqual(9.99m, (decimal)spec.OrderBy.Compile()(Product("Hat", price: 9.99m)));
            Assert.IsNull(spec.OrderByDescending);
        }

        [TestMethod]
        public void Sort_PriceDesc_OrdersByPriceDescending()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(new ProductSpecParams { Sort = "priceDesc" });

            Assert.IsNotNull(spec.OrderByDescending);
            Assert.AreEqual(9.99m, (decimal)spec.OrderByDescending.Compile()(Product("Hat", price: 9.99m)));
        }

        [TestMethod]
        public void Sort_UnknownValue_FallsBackToName()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(new ProductSpecParams { Sort = "colour" });

            Assert.AreEqual("Hat", (string)spec.OrderBy.Compile()(Product("Hat")));
            Assert.IsNull(spec.OrderByDescending);
        }

        // -------------------------------------------------------------- filtering

        [TestMethod]
        public void Criteria_WithNoFilters_MatchesEveryProduct()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(new ProductSpecParams());

            Assert.IsTrue(Matches(spec, Product("Anything", brandId: 7, typeId: 9)));
        }

        [TestMethod]
        public void Criteria_Search_MatchesAnywhereInTheName_CaseInsensitively()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(new ProductSpecParams { Search = "HAT" });

            Assert.IsTrue(Matches(spec, Product("Core Purple Hat")));
            Assert.IsFalse(Matches(spec, Product("Angular Speedster Board")));
        }

        [TestMethod]
        public void Criteria_BrandFilter_RejectsOtherBrands()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(new ProductSpecParams { BrandId = 2 });

            Assert.IsTrue(Matches(spec, Product("Hat", brandId: 2)));
            Assert.IsFalse(Matches(spec, Product("Hat", brandId: 1)));
        }

        [TestMethod]
        public void Criteria_BrandAndType_MustBothMatch()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(new ProductSpecParams { BrandId = 1, TypeId = 1 });

            Assert.IsTrue(Matches(spec, Product("Hat", brandId: 1, typeId: 1)));
            Assert.IsFalse(Matches(spec, Product("Hat", brandId: 1, typeId: 2)), "right brand, wrong type");
            Assert.IsFalse(Matches(spec, Product("Hat", brandId: 2, typeId: 1)), "right type, wrong brand");
        }

        // ------------------------------------------------------------------ by id

        [TestMethod]
        public void ById_MatchesOnlyThatProduct_AndDoesNotPage()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(3);

            Assert.IsTrue(spec.Criteria.Compile()(new Product { Id = 3 }));
            Assert.IsFalse(spec.Criteria.Compile()(new Product { Id = 4 }));
            Assert.IsFalse(spec.IsPagingEnabled);
        }

        [TestMethod]
        public void BothConstructors_IncludeBrandAndType_SoTheDtoCanShowTheirNames()
        {
            Assert.AreEqual(2, new ProductsWithTypesAndBrandsSpecification(new ProductSpecParams()).Includes.Count);
            Assert.AreEqual(2, new ProductsWithTypesAndBrandsSpecification(3).Includes.Count);
        }
    }
}
