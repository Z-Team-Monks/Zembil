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
            CreateMap<Product, ProductReviewDto>();
            CreateMap<ReviewDto, Review>()
                .ForMember(
                    dest => dest.ReviewDate,
                    opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<Review, ReviewToReturnDto>();
            CreateMap<ReviewToUpdateDto, Review>();
            CreateMap<User, UserGetDto>();
            CreateMap<List<User>, List<UserGetDto>>();
            CreateMap<Shop, ShopReturnDto>();
            CreateMap<ShopReturnDto, Shop>();
            CreateMap<List<Shop>, List<ShopReturnDto>>();


        }
    }
}
