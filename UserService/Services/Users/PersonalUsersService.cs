using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Services
{
    public class PersonalUsersService : IPersonalUsersService
    {
        public PersonalUserCreatedConfirmation CreateAdmin(PersonalUser user)
        {
            throw new NotImplementedException();
        }

        public PersonalUserCreatedConfirmation CreateUser(PersonalUser user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public PersonalUser GetUserByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        public List<PersonalUser> GetUsers(string city = null, string username = null)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(PersonalUser user)
        {
            throw new NotImplementedException();
        }
    }
}
