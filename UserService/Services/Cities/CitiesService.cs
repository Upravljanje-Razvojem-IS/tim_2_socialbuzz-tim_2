using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Entities;
using UserService.Exceptions;

namespace UserService.Services.Cities
{
    public class CitiesService : ICitiesService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IPersonalUserRepository _personalUserRepository;
        private readonly ICorporationUserRepository _corporationUserRepository;
        public CitiesService(ICityRepository cityRepository, ICorporationUserRepository corporationUserRepository, IPersonalUserRepository personalUserRepository)
        {
            _cityRepository = cityRepository;
            _corporationUserRepository = corporationUserRepository;
            _personalUserRepository = personalUserRepository;
        }
        public CityCreatedConfirmation CreateCity(City city)
        {
            CityCreatedConfirmation createdCity = _cityRepository.CreateCity(city);
            _cityRepository.SaveChanges();
            return createdCity;
        }

        public void DeleteCity(Guid cityId)
        {
            List<PersonalUser> pUsers = _personalUserRepository.GetUsersWithCity(cityId);
            if (pUsers.Count > 0)
            {
                throw new ReferentialConstraintViolationException("Value of city is referenced in another table");
            }
            List<Corporation> cUsers = _corporationUserRepository.GetUsersWithCity(cityId);
            if (cUsers.Count > 0)
            {
                throw new ReferentialConstraintViolationException("Value of city is referenced in another table");
            }
            _cityRepository.DeleteCity(cityId);
            _cityRepository.SaveChanges();
        }

        public List<City> GetCities(string cityName)
        {
            return _cityRepository.GetCities(cityName);
        }

        public City GetCityByCityId(Guid cityId)
        {
            return _cityRepository.GetCityByCityId(cityId);
        }

        public void UpdateCity(City city)
        {
            throw new NotImplementedException();
        }
    }
}
