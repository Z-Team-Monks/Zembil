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

            CreateMap<ShopBatchGetDto, Shop>();
            CreateMap<Shop,ShopBatchGetDto>()
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => getStatusForShop(src.IsActive))); 

            CreateMap<ShopCreateDto, Shop>();
            CreateMap<Shop, ShopCreateDto>();

            CreateMap<Shop, ShopReturnDto>();

            CreateMap<Shop, ShopDto>();
            CreateMap<ShopDto, Shop>();

            CreateMap<ShopChangeDto, Shop>();
            CreateMap<Shop, ShopChangeDto>();

            CreateMap<ShopReturnDto, Shop>();
            CreateMap<List<Shop>, List<ShopReturnDto>>();

            CreateMap<Shop, ShopDto>()
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => getStatusForShop(src.IsActive)));

            CreateMap<Product, ProductGetBatchDto>();
            CreateMap<ProductGetBatchDto,Product>();

            CreateMap<Product, ProductUpdateDto>();
            CreateMap<ProductUpdateDto,Product>();

            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();



            CreateMap<CategoryDto, Category>();

            CreateMap<NewLocationDto, Location>();
            CreateMap<Location,NewLocationDto>();

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
