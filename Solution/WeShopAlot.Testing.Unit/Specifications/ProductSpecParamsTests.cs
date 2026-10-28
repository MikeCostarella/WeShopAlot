using WeShopAlot.Data.Specifications;

namespace WeShopAlot.Testing.Unit.Specifications
{
    /// <summary>
    /// ProductSpecParams is the query string of GET /api/product, bound to an object.
    /// Two of its setters contain rules; each rule is a test.
    /// </summary>
    [TestClass]
    public class ProductSpecParamsTests
    {
        [TestMethod]
        public void PageSize_DefaultsToSix()
        {
            var p = new ProductSpecParams();

            Assert.AreEqual(6, p.PageSize);
        }

        [TestMethod]
        public void PageIndex_DefaultsToTheFirstPage()
        {
            var p = new ProductSpecParams();

            Assert.AreEqual(1, p.PageIndex);
        }

        [TestMethod]
        public void PageSize_AboveFifty_IsCappedAtFifty()
        {
            var p = new ProductSpecParams();

            p.PageSize = 500;

            Assert.AreEqual(50, p.PageSize, "a client cannot ask for the whole catalog in one page");
        }

        [TestMethod]
        public void PageSize_AtOrBelowFifty_IsKeptAsAsked()
        {
            var p = new ProductSpecParams();

            p.PageSize = 50;

            Assert.AreEqual(50, p.PageSize);
        }

        [TestMethod]
        public void Search_IsLowerCased_SoTheCriteriaCanCompareLowerToLower()
        {
            var p = new ProductSpecParams();

            p.Search = "Purple HAT";

            Assert.AreEqual("purple hat", p.Search);
        }

        [TestMethod]
        public void Search_Null_IsAllowed()
        {
            // The setter used to call value.ToLower() unguarded, so Search = null threw
            // NullReferenceException. Model binding never assigns null, which is why the
            // API never hit it - this test did, the first time it ran.
            var p = new ProductSpecParams();

            p.Search = null;

            Assert.IsNull(p.Search);
        }
    }
}
