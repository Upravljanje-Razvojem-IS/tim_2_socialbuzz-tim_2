using AutoMapper;
using RatingService.DTO;
using RatingService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
