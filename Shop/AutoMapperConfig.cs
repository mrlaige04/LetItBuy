using AutoMapper;
using Shop.BLL.DTO;
using Shop.DAL.Data.Entities;
using Shop.Models.UserModels;

namespace Shop.UI
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<ApplicationUser, UserDTO>();
            CreateMap<Item, ItemDTO>();
            CreateMap<ApplicationUser, ProfileViewModel>();
        }
    }
}
