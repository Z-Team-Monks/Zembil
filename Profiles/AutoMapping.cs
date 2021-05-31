using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zembil.Models;
using Zembil.Utils;
using Zembil.Views;

namespace Zembil.Profiles
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<User, UserCreateDto>();
            CreateMap<UserCreateDto, User>();

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

            CreateMap<ShopCreateDto, Shop>();
            CreateMap<Shop, ShopCreateDto>();

            CreateMap<ShopBatchGetDto, Shop>();
            CreateMap<Shop, ShopBatchGetDto>()
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => HelperMethods.getInstanceEmpty().getStatusForShop(src.IsActive)));

            CreateMap<ShopReturnDto, Shop>();
            CreateMap<List<Shop>, List<ShopReturnDto>>();

            CreateMap<ShopLocation, LocationDto>();

            CreateMap<Shop, ShopDto>()
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => HelperMethods.getInstanceEmpty().getStatusForShop(src.IsActive)));

            CreateMap<AdsCreateDto, Ads>();
            CreateMap<Ads, AdsCreateDto>();

            CreateMap<NewLocationDto, LocationDto>();
            CreateMap<LocationDto, NewLocationDto>();

            CreateMap<Product, ProductGetBatchDto>();
            CreateMap<ProductGetBatchDto, Product>();

            CreateMap<Product, ProductUpdateDto>();
            CreateMap<ProductUpdateDto, Product>();

            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();

            CreateMap<CategoryDto, Category>();
            CreateMap<Category, CategoryDto>();

            CreateMap<NewLocationDto, ShopLocation>();
            CreateMap<ShopLocation, NewLocationDto>();

            CreateMap<WishListAddDto, WishListItem>();
            CreateMap<WishListItem, WishListAddDto>();

            CreateMap<WishListDto, WishListItem>();
            CreateMap<WishListItem, WishListDto>();

            CreateMap<Notification, NotificationDto>();
            CreateMap<NotificationDto, Notification>();
        }
    }
}
