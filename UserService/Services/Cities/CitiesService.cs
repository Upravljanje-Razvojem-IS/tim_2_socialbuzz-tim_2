using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Entities;

namespace UserService.Services.Cities
{
    public class CitiesService : ICitiesService
    {
        private readonly ICityRepository _cityRepository;
        public CitiesService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        public CityCreatedConfirmation CreateCity(City city)
        {
            CityCreatedConfirmation createdCity = _cityRepository.CreateCity(city);
            _cityRepository.SaveChanges();
            return createdCity;
        }

        public void DeleteCity(Guid cityId)
        {
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
