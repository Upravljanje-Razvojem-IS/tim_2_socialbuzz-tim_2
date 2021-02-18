﻿using System;
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
            return context.PersonalUser.FirstOrDefault(e => e.UserId == userId);
        }

        public List<PersonalUser> GetUsers(string city = null)
        {
            return context.PersonalUser.Where(e => city == null || e.City.CityName == city).ToList();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(PersonalUser user)
        {
            throw new NotImplementedException();
        }
    }
}
