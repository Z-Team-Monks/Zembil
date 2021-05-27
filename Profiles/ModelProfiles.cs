using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zembil.Profiles
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<Models.Product, Views.ProductCreateDto>();
            CreateMap<Views.ProductCreateDto, Models.Product>();
            CreateMap<Views.ReviewDto, Models.Review>()
                .ForMember(
                    dest => dest.ReviewDate,
                    opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<Models.Review, Views.ReviewToReturnDto>();
            CreateMap<Views.ReviewToUpdateDto, Models.Review>();
            CreateMap<Models.User, Views.UserGetDto>();

        }
    }
}
