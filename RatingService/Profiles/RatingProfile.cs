using AutoMapper;
using RatingService.DTO;
using RatingService.Entities;

namespace RatingService.Profiles
{
    public class RatingProfile : Profile
    {

        public RatingProfile()
        {
            CreateMap<RatingModifyingDTO, Rating>();

            CreateMap<RatingCreationDTO, Rating>();

            CreateMap<Rating, RatingDTO>();

            CreateMap<Rating, Rating>();
        }
    }
}
