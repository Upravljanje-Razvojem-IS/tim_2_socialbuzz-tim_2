using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Services.Users
{
    public interface ICorporationUsersService
    {
        List<Corporation> GetUsers(string city = null, string username = null);
        Corporation GetUserByUserId(Guid userId);
        CorporationUserCreatedConfirmation CreateUser(Corporation user);
        void UpdateUser(Corporation user);
        void DeleteUser(Guid userId);
    }
}
