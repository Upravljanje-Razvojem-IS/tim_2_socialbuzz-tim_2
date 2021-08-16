using AutoMapper;
using RatingService.DTO;
using RatingService.Entities;

namespace RatingService.Profiles
{
    public class RatingTypeProfile: Profile
    {

        public RatingTypeProfile()
        {
            CreateMap<RatingTypeModifyingDTO, RatingType>();

            CreateMap<RatingTypeCreationDTO, RatingType>();

            CreateMap<RatingType, RatingTypeDTO>();

            CreateMap<RatingType, RatingType>();
        }
    }
}
