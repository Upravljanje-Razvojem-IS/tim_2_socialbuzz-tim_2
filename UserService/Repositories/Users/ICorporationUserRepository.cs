using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Data
{
    public interface ICorporationUserRepository
    {
        List<Corporation> GetUsers(string city = null, string username = null);
        List<Corporation> GetUsersWithRole(Guid id);
        List<Corporation> GetUsersWithCity(Guid id);
        Corporation GetUserByUserId(Guid userId);
        Corporation GetUserWithEmail(string email);

        CorporationUserCreatedConfirmation CreateUser(Corporation user);
        void UpdateUser(Corporation user);
        void DeleteUser(Guid userId);
        bool SaveChanges();
    }
}
