using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UserService.Dtos;
using UserService.Entities;

namespace UserService.Profiles
{
    public class CorporationUserProfile: Profile
    {
        public CorporationUserProfile()
        {
            CreateMap<Corporation, CorporationDto>().ForMember(
               dest => dest.City,
               opt => opt.MapFrom(src => $"{src.City.CityName}"))
               .ForMember(
               dest => dest.Role,
               opt => opt.MapFrom(src => $"{src.Role.RoleName}")
               );
        }
       
    }
}
