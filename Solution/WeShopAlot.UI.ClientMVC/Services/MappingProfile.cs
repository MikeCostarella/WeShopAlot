using AutoMapper;
using WeShopAlot.Shared.Dtos;
using WeShopAlot.UI.ClientMVC.Models;

namespace WeShopAlot.UI.ClientMVC.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<ProductToReturnDto, ProductViewModel>();
        }
    }
}
