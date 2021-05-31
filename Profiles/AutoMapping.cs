using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zembil.Models;
using Zembil.Views;

namespace Zembil.Profiles
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<User, UserCreateDto>();
            CreateMap<UserCreateDto,User>();

            CreateMap<Product, ProductCreateDto>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ReviewDto, Review>()
                .ForMember(
                    dest => dest.ReviewDate,
                    opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<Review, ReviewToReturnDto>();
            CreateMap<ReviewToUpdateDto, Review>();

            CreateMap<User, UserGetDto>();
            CreateMap<List<User>, List<UserGetDto>>();

            CreateMap<Shop, ShopReturnDto>();
            CreateMap<ShopDto, Shop>();

            CreateMap<ShopChangeDto, Shop>();
            CreateMap<Shop, ShopChangeDto>();

            CreateMap<ShopReturnDto, Shop>();
            CreateMap<List<Shop>, List<ShopReturnDto>>();

            CreateMap<ShopLocation, LocationDto>();

            CreateMap<Shop, ShopDto>()
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => getStatusForShop(src.IsActive)));

            CreateMap<AdsCreateDto, Ads>();
            CreateMap<Ads,AdsCreateDto>();

            CreateMap<NewLocationDto, LocationDto>();
            CreateMap<LocationDto,NewLocationDto>();
        }

        public string getStatusForShop(bool? isActive)
        {
            switch (isActive)
            {
                case null:
                    return "Pending";
                case true:
                    return "Approved";
                case false:
                    return "Declined";
            }
        }
    }
}
