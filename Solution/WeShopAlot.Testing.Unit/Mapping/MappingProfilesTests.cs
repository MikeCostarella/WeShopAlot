using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Shared.Enumerations;
using WeShopAlot.Shared.Dtos;
using WeShopAlot.WebAPI.Helpers;

namespace WeShopAlot.Testing.Unit.Mapping
{
    /// <summary>
    /// Every ForMember line in MappingProfiles carries a comment about a bug it fixed.
    /// AutoMapper runs without the web host, so each of those fixes gets a test here.
    /// </summary>
    [TestClass]
    public class MappingProfilesTests
    {
        private static IMapper mapper;

        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            // The two Url resolvers ask for IConfiguration["ApiUrl"]; hand them a tiny in-memory one.
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { ["ApiUrl"] = "https://api.test/" })
                .Build();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
                cfg.ConstructServicesUsing(type => Activator.CreateInstance(type, configuration));
            }, NullLoggerFactory.Instance);

            mapper = config.CreateMapper();
        }

        private static Order SampleOrder(DeliveryMethod delivery = null) => new()
        {
            Id = 42,
            BuyerEmail = "ada@test.com",
            OrderDate = new DateTime(2026, 10, 28, 21, 0, 0, DateTimeKind.Unspecified), // how SQL Server hands it back
            Subtotal = 400m,
            DeliveryMethod = delivery,
            Status = new OrderStatus { Name = OrderStatusEnum.Pending, Description = "Pending" },
            ShipToAddress = new Address { FirstName = "Ada", LastName = "Lovelace", AddressLine1 = "1 Main St", City = "Girard", State = "OH", ZipCode = "44420" },
            OrderItems = new List<OrderItem>
            {
                new() { ItemOrdered = new ProductItemOrdered { ProductItemId = 1, ProductName = "Angular Speedster Board", PictureUrl = "images/products/sb-ang1.png" }, Price = 200m, Quantity = 2 },
            },
        };

        private static readonly DeliveryMethod Ups = new() { Id = 1, ShortName = "UPS1", Price = 10m };

        // ----------------------------------------------------------------- orders

        [TestMethod]
        public void Order_Total_IsSubtotalPlusShipping()
        {
            var dto = mapper.Map<OrderToReturnDto>(SampleOrder(Ups));

            Assert.AreEqual(410m, dto.Total, "Total used to come back 0 because Order has no GetTotal()");
            Assert.AreEqual(10m, dto.ShippingPrice);
            Assert.AreEqual("UPS1", dto.DeliveryMethod);
        }

        [TestMethod]
        public void Order_WithoutDeliveryMethodLoaded_TotalIsJustTheSubtotal()
        {
            var dto = mapper.Map<OrderToReturnDto>(SampleOrder(delivery: null));

            Assert.AreEqual(400m, dto.Total);
        }

        [TestMethod]
        public void Order_OrderDate_IsMarkedUtc_SoTheJsonEndsInZ()
        {
            var dto = mapper.Map<OrderToReturnDto>(SampleOrder(Ups));

            Assert.AreEqual(DateTimeKind.Utc, dto.OrderDate.Kind, "a 9 PM order in Ohio once displayed as 1 AM the next day");
            Assert.AreEqual(21, dto.OrderDate.Hour, "the clock value itself must not shift");
        }

        [TestMethod]
        public void Order_Status_IsTheLookupRowsDescription_NotTheClassName()
        {
            var dto = mapper.Map<OrderToReturnDto>(SampleOrder(Ups));

            Assert.AreEqual("Pending", dto.Status);
        }

        [TestMethod]
        public void OrderItem_FlattensItemOrdered_AndPrefixesThePictureUrl()
        {
            var dto = mapper.Map<OrderToReturnDto>(SampleOrder(Ups));

            var item = dto.OrderItems.Single();
            Assert.AreEqual(1, item.ProductId);
            Assert.AreEqual("Angular Speedster Board", item.ProductName);
            Assert.AreEqual("https://api.test/images/products/sb-ang1.png", item.PictureUrl);
            Assert.AreEqual(2, item.Quantity);
        }

        // --------------------------------------------------------------- products

        [TestMethod]
        public void Product_BrandAndType_MapToTheirNames()
        {
            var product = new Product
            {
                Id = 3, Name = "Core Purple Hat", Price = 10m,
                ProductBrand = new ProductBrand { Name = "NetCore" },
                ProductType = new ProductType { Name = "Hats" },
                PictureUrl = "images/products/hat-core1.png",
            };

            var dto = mapper.Map<ProductToReturnDto>(product);

            Assert.AreEqual("NetCore", dto.ProductBrand);
            Assert.AreEqual("Hats", dto.ProductType);
            Assert.AreEqual("https://api.test/images/products/hat-core1.png", dto.PictureUrl);
        }

        [TestMethod]
        public void Product_WithoutPicture_HasNullPictureUrl_NotJustThePrefix()
        {
            var dto = mapper.Map<ProductToReturnDto>(new Product { Name = "Unpictured", ProductBrand = new ProductBrand(), ProductType = new ProductType() });

            Assert.IsNull(dto.PictureUrl);
        }

        // -------------------------------------------------------------- addresses

        [TestMethod]
        public void Address_RoundTrip_KeepsNamesStreetAndZip()
        {
            var dto = new AddressDto { FirstName = "Ada", LastName = "Lovelace", Street = "1 Main St", City = "Girard", State = "OH", Zipcode = "44420" };

            var entity = mapper.Map<Address>(dto);
            var back = mapper.Map<AddressDto>(entity);

            Assert.AreEqual("1 Main St", entity.AddressLine1, "Street and AddressLine1 are spelled differently on each side");
            Assert.AreEqual(string.Empty, entity.AddressLine2);
            Assert.AreEqual("44420", entity.ZipCode);
            Assert.AreEqual("Ada", back.FirstName, "the name used to be dropped because Address had no name columns");
            Assert.AreEqual("Lovelace", back.LastName);
            Assert.AreEqual("44420", back.Zipcode);
        }
    }
}
