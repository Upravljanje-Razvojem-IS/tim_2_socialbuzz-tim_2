using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Dtos;
using UserService.Entities;

namespace UserService.Profiles
{
    public class PersonalUserProfile: Profile
    {
        public PersonalUserProfile()
        {
            CreateMap<PersonalUser, PersonalUserDto>().ForMember(
                dest => dest.FirstAndLastName,
                opt => opt.MapFrom(src => $"{src.FirstName}{src.LastName}"))
                .ForMember(
                dest => dest.City,
                opt => opt.MapFrom(src => $"{src.City.CityName}"))
                .ForMember(
                dest => dest.Role,
                opt => opt.MapFrom(src => $"{src.Role.RoleName}")
                ); 
        }
    }
}
