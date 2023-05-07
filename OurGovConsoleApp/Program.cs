using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MeShopAlot.Data.ConsoleApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            Environment.Exit(0);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}