using AutoMapper;
using WeShopAlot.Data.Models;
using WeShopAlot.Shared.Dtos;

namespace WeShopAlot.WebAPI.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
            // The Address entity and AddressDto use different names (AddressLine1/Street, ZipCode/Zipcode),
            // so those members are spelled out in both directions; FirstName and LastName map by name.
            CreateMap<Address, AddressDto>()
                .ForMember(d => d.Street, o => o.MapFrom(s => s.AddressLine1))
                .ForMember(d => d.Zipcode, o => o.MapFrom(s => s.ZipCode));
            CreateMap<AddressDto, Address>()
                .ForMember(d => d.AddressLine1, o => o.MapFrom(s => s.Street))
                .ForMember(d => d.AddressLine2, o => o.MapFrom(s => string.Empty))
                .ForMember(d => d.ZipCode, o => o.MapFrom(s => s.Zipcode));
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            //CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price))
                // OrderDate is saved as UTC, but SQL Server hands it back with no time zone, so the JSON had no "Z"
                // and the browser showed UTC as local time (a 9 PM order in Ohio displayed as 1 AM the next day).
                .ForMember(d => d.OrderDate, o => o.MapFrom(s => DateTime.SpecifyKind(s.OrderDate, DateTimeKind.Utc)))
                // Status is a lookup row; without this AutoMapper printed the class name.
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.Description))
                // Order has no GetTotal() for AutoMapper to find, so Total always came back 0.
                .ForMember(d => d.Total, o => o.MapFrom(s => s.Subtotal + (s.DeliveryMethod == null ? 0 : s.DeliveryMethod.Price)));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
        }
    }
}
