using AutoMapper;
using RatingService.DTO;
using RatingService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
