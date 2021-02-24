using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UserService.Dtos;
using UserService.Dtos.Users;
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

            CreateMap<Corporation, UserInfoDto>().ForMember(
                dest => dest.AccountType,
                opt => opt.MapFrom(src => "corporationUser")).ForMember(
                dest => dest.City,
                opt => opt.MapFrom(src => $"{src.City.CityName}"))
                .ForMember(
                dest => dest.Role,
                opt => opt.MapFrom(src => $"{src.Role.RoleName}")
                );

            CreateMap<CorporationUserCreationDto, Corporation>().ForMember(
                dest => dest.CityId,
                opt => opt.MapFrom(src => src.CityId));
            CreateMap<Corporation, CorporationUserCreatedConfirmation>();
            CreateMap<CorporationUserCreatedConfirmation, CorporationUserCreatedConfirmationDto>();
            CreateMap<CorporationUserUpdateDto, Corporation>();
            CreateMap<Corporation, Corporation>();
        }
       
    }
}
