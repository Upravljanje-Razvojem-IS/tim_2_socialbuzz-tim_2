using AutoMapper;
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
        private readonly IMapper mapper;

        public CityRepository(UserDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public CityCreatedConfirmation CreateCity(City city)
        {
            var createdCity = context.Add(city);
            return mapper.Map<CityCreatedConfirmation>(createdCity.Entity);
        }

        public void DeleteCity(Guid cityId)
        {
            var city = GetCityByCityId(cityId);
            context.Remove(city);
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
            return context.SaveChanges() > 0;
        }

        public void UpdateCity(City city)
        {
            throw new NotImplementedException();
        }
    }
}
