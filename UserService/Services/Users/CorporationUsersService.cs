using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Entities;
using UserService.Exceptions;

namespace UserService.Services.Users
{
    public class CorporationUsersService : ICorporationUsersService
    {

        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IPersonalUserRepository _personalUserRepository;
        private readonly UserManager<AccountInfo> _userManager;
        private readonly ICorporationUserRepository _corporationUserRepository;
        public CorporationUsersService(ICorporationUserRepository corporationUserRepository, IMapper mapper,
            IRoleRepository roleRepository, ICityRepository cityRepository,
            IPersonalUserRepository personalUserRepository, UserManager<AccountInfo> userManager)
        {
            _corporationUserRepository = corporationUserRepository;
            _personalUserRepository = personalUserRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        public async Task<CorporationUserCreatedConfirmation> CreateUser(Corporation user, string password)
        {
            if (!CheckUniqueUsername(user.Username))
            {
                //Unique violation
                throw new UniqueValueViolationException("Username should be unique");
            }
            if (!CheckCity(user.CityId))
            {
                throw new ForeignKeyConstraintViolationException("Foreign key constraint violated");

            }
            CorporationUserCreatedConfirmation userCreated = _corporationUserRepository.CreateUser(user);
            _corporationUserRepository.SaveChanges();

            //Adding to identityuserdbcontext tables
            string username = string.Join("", user.Username.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            var acc = new AccountInfo(username, user.Email, userCreated.UserId);
            IdentityResult result = await _userManager.CreateAsync(acc, password);
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(acc, "Regular user").Wait();
            }
            else
            {
                _corporationUserRepository.DeleteUser(userCreated.UserId);
                throw new ExectionException("Erorr trying to create user");

            }
            return null;
        }

        public async void DeleteUser(Guid userId)
        {
            _corporationUserRepository.DeleteUser(userId);
            _corporationUserRepository.SaveChanges();
            //Delete from identity table
            var acc = await _userManager.FindByIdAsync(userId.ToString());
            await _userManager.DeleteAsync(acc);
        }

        public Corporation GetUserByUserId(Guid userId)
        {
            return _corporationUserRepository.GetUserByUserId(userId);
        }

        public List<Corporation> GetUsers(string city = null, string username = null)
        {
            return _corporationUserRepository.GetUsers(city, username);
        }

        public async void UpdateUser(Corporation updatedUser, Corporation userWithId) 
        {
            if (!CheckUniqueUsername(updatedUser.Username))
            {
                //Unique violation
                throw new UniqueValueViolationException("Username should be unique");
            }
            City city = _cityRepository.GetCityByCityId(updatedUser.CityId);
            if (city == null)
            {
                throw new ForeignKeyConstraintViolationException("Foreign key constraint violated");

            }
            updatedUser.RoleId = userWithId.RoleId;
            updatedUser.Email = userWithId.Email;
            updatedUser.Role = _roleRepository.GetRoleByRoleId(userWithId.RoleId);
            updatedUser.City = city;
            updatedUser.UserId = userWithId.UserId;
            _mapper.Map(updatedUser, userWithId);
            _corporationUserRepository.SaveChanges();

            //Updating identity table
            AccountInfo acc = await _userManager.FindByIdAsync(userWithId.UserId.ToString());
            string username = string.Join("", updatedUser.Username.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            acc.UserName = username;
            await _userManager.UpdateAsync(acc);
        }

        private bool CheckUniqueUsername(string username)
        {
            var users = _corporationUserRepository.GetUsers(null, username);
            if (users != null && users.Count > 0)
            {
                //Unique violation
                return false;
            }
            var persUsers = _personalUserRepository.GetUsers(null, username);
            if (persUsers != null && persUsers.Count > 0)
            {
                //Unique violation
                return false;
            }
            return true;
        }

        private bool CheckCity(Guid cityId)
        {
            return (_cityRepository.GetCityByCityId(cityId) != null);
        }
    }
}
