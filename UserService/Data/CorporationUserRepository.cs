using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Data
{
    public class CorporationUserRepository : ICorporationUserRepository
    {
        private readonly UserDbContext context;

        public CorporationUserRepository(UserDbContext context)
        {
            this.context = context;
        }

        public UserCreatedConfirmation CreateUser(Corporation user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Corporation GetUserByUserId(Guid userId)
        {
            return context.Corporation.Include(user => user.City).Include(user => user.Role).FirstOrDefault(e => e.UserId == userId);

        }

        public List<Corporation> GetUsers(string city = null, string username = null)
        {
            return context.Corporation.Include(user => user.City).Include(user => user.Role).
                Where(e => city == null || e.City.CityName == city).Where(e => username == null || e.Username.Equals(username)).
                ToList();
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateUser(Corporation user)
        {
            throw new NotImplementedException();
        }
    }
}
