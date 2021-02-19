using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Data
{
    public interface ICityRepository
    {
        List<City> GetCities();
        City GetCityByCityId(Guid cityId);
        CityCreatedConfirmation CreateCity(City city);
        void UpdateCity(City cit);
        void DeleteCity(Guid cityId);
        bool SaveChanges();
    }
}
