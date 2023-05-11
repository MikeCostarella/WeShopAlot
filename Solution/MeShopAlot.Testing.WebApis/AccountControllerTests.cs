namespace MeShopAlot.Testing.WebApis
{
    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]
        public void TestLogin()
        {
            using (var client = new HttpClient())
            {
                //Send HTTP requests from here.
                client.BaseAddress = new Uri("http://localhost:7244/api/");
            }
        }
    }
}