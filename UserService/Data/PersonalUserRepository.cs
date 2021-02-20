using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Data
{
    public class PersonalUserRepository : IPersonalUserRepository
    {
        private readonly UserDbContext context;

        public PersonalUserRepository(UserDbContext context)
        {
            this.context = context;
            this.context.ChangeTracker.LazyLoadingEnabled = false;
        }

        public UserCreatedConfirmation CreateUser(PersonalUser user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public PersonalUser GetUserByUserId(Guid userId)
        {
            return context.PersonalUser.Include(user => user.City).Include(user => user.Role).FirstOrDefault(e => e.UserId == userId);
        }

        public PersonalUser GetUserByUsername(string username)
        {
            return context.PersonalUser.Include(user => user.City).Include(user => user.Role).Where(e => e.Username.ToLowerInvariant() == username.ToLowerInvariant()).FirstOrDefault();

        }

        public List<PersonalUser> GetUsers(string city = null, string username = null)
        {
            return context.PersonalUser.Include(user => user.City).Include(user => user.Role).
              Where(e => city == null || e.City.CityName == city).Where(e => username == null || e.Username.Equals(username)).
              ToList();
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateUser(PersonalUser user)
        {
            throw new NotImplementedException();
        }
    }
}
