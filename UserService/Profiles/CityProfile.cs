using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Dtos;
using UserService.Dtos.Cities;
using UserService.Entities;

namespace UserService.Profiles
{
    public class CityProfile: Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityDto>();
            CreateMap<CityCreationDto, City>();
            CreateMap<City, CityCreatedConfirmation>();
            CreateMap<CityCreatedConfirmation, CityCreatedConfirmationDto>();
            CreateMap<CityUpdateDto, City>();
            CreateMap<City, City>();
        }
    }
}
