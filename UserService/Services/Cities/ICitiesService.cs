using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Services.Cities
{
    public interface ICitiesService
    {
        List<City> GetCities(string cityName);
        City GetCityByCityId(Guid cityId);
        CityCreatedConfirmation CreateCity(City city);
        void UpdateCity(City city);
        void DeleteCity(Guid cityId);
    }
}
