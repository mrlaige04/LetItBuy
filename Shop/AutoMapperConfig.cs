using AutoMapper;
using Shop.BLL.DTO;
using Shop.DAL.Data.Entities;
using Shop.Models.UserModels;
using Shop.UI.Models.ViewDTO;

namespace Shop.UI
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<ApplicationUser, UserDTO>();
            CreateMap<Item, ItemDTO>();
                
            CreateMap<ApplicationUser, ProfileViewModel>();

            CreateMap<NumberCriteriaValue, NumberCriteriaViewModel>();

            CreateMap<StringCriteriaValue, StringCriteriaViewModel>();

            CreateMap<Category, CategoryDTO>()
                .ForMember(x => x.NumberCriterias, opt =>
                    opt.MapFrom(x => x.NumberCriteriasValues.OrderBy(y => y.CriteriaName)
                        .OrderBy(x => x.Value)
                        .GroupBy(x => new GroupAbleCriteria(x.CriteriaID, x.CriteriaName, x.multiple))
                        .Select(x => new NumberCriteriaViewModel(x.Key, x))))
                .ForMember(x => x.StringCriterias, opt =>
                    opt.MapFrom(x => x.StringCriteriasValues.OrderBy(y => y.CriteriaName)
                        .OrderBy(x => x.Value)
                        .GroupBy(x => new GroupAbleCriteria(x.CriteriaID, x.CriteriaName, x.multiple))
                        .Select(x => new StringCriteriaViewModel(x.Key, x))));

            CreateMap<BuyViewModel, DeliveryInfo>();
            CreateMap<BuyViewModel, Order>();
        }
    }

}
