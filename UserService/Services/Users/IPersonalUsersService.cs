using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Services
{
    public interface IPersonalUsersService
    {
        List<PersonalUser> GetUsers(string city = null, string username = null);
        PersonalUser GetUserByUserId(Guid userId);
        Task<PersonalUserCreatedConfirmation> CreateUser(PersonalUser user, string password);
        Task<PersonalUserCreatedConfirmation> CreateAdmin(PersonalUser user, string password);
        void UpdateUser(PersonalUser updatedUser, PersonalUser userWithId);
        void DeleteUser(Guid userId);
    }
}
