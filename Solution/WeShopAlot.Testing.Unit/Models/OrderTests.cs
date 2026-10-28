using WeShopAlot.Data.Models;
using WeShopAlot.Data.Shared.Enumerations;

namespace WeShopAlot.Testing.Unit.Models
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
        public void NewOrder_StartsPending()
        {
            // OrderStatus rows are seeded with Ids 1-3. The old default of 0 broke the foreign key and
            // every checkout failed on SaveChanges. The in-memory database used by the API tests does
            // not enforce foreign keys, so this is the only test that can catch a regression.
            Assert.AreEqual((int)OrderStatusEnum.Pending, new Order().StatusId);
        }

        [TestMethod]
        public void NewOrder_IsStampedInUtc()
        {
            var order = new Order();

            Assert.AreEqual(DateTimeKind.Utc, order.OrderDate.Kind, "the browser converts UTC to local; a local stamp would be converted twice");
        }
    }
}
