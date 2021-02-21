using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Data
{
    public class CityRepository : ICityRepository
    {
        private readonly UserDbContext context;

        public CityRepository(UserDbContext context)
        {
            this.context = context;
        }

        public CityCreatedConfirmation CreateCity(City city)
        {
            throw new NotImplementedException();
        }

        public void DeleteCity(Guid cityId)
        {
            throw new NotImplementedException();
        }

        public List<City> GetCities(string cityName)
        {
            return context.City.Where(c => cityName == null || c.CityName == cityName).ToList();
        }

        public City GetCityByCityId(Guid cityId)
        {
            return context.City.FirstOrDefault(e => e.CityId == cityId);

        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void UpdateCity(City cit)
        {
            throw new NotImplementedException();
        }
    }
}
