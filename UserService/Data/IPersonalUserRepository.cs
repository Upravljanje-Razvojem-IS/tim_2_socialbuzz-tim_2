using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Data
{
    public interface IPersonalUserRepository
    {
        List<PersonalUser> GetUsers(string city = null, string username = null);
        PersonalUser GetUserByUserId(Guid userId);
        UserCreatedConfirmation CreateUser(PersonalUser user);
        void UpdateUser(PersonalUser user);
        void DeleteUser(Guid userId);
        bool SaveChanges();
        
    }
}
