using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Entities;

namespace UserService.Services.Users
{
    public class CorporationUsersService : ICorporationUsersService
    {
        private readonly ICorporationUserRepository _corporationUserRepository;
        public CorporationUsersService(ICorporationUserRepository corporationUserRepository)
        {
            _corporationUserRepository = corporationUserRepository;
        }

        public CorporationUserCreatedConfirmation CreateUser(Corporation user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Corporation GetUserByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        public List<Corporation> GetUsers(string city = null, string username = null)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(Corporation user)
        {
            throw new NotImplementedException();
        }
    }
}
