using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Entities;
using UserService.Exceptions;

namespace UserService.Services
{
    public class PersonalUsersService : IPersonalUsersService
    {
        private readonly IPersonalUserRepository _personalUserRepository;
        private readonly ICorporationUserRepository _corporationUserRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ICityRepository _cityRepository;
        private readonly UserManager<AccountInfo> _userManager;
        private readonly IMapper _mapper;



        public PersonalUsersService(IMapper mapper, IPersonalUserRepository personalUserRepository, IRoleRepository roleRepository, ICityRepository cityRepository,
            ICorporationUserRepository corporationUserRepository, UserManager<AccountInfo> userManager)
        {
            _personalUserRepository = personalUserRepository;
            _corporationUserRepository = corporationUserRepository;
            _cityRepository = cityRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _mapper = mapper;

        }

        public async Task<PersonalUserCreatedConfirmation> CreateAdmin(PersonalUser user, string password)
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
            //Adding to userdbcontext tables
            PersonalUserCreatedConfirmation userCreated = _personalUserRepository.CreateAdmin(user);
            _personalUserRepository.SaveChanges();

            //Adding to identityuserdbcontext tables
            string username = string.Join("", user.Username.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            var acc = new AccountInfo(username, user.Email, userCreated.UserId);
            IdentityResult result = await _userManager.CreateAsync(acc, password);
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(acc, "Admin").Wait();
            }
            else
            {
                _personalUserRepository.DeleteUser(userCreated.UserId);
                throw new ExectionException("Erorr trying to create user");

            }
            return userCreated;

        }

        public async Task<PersonalUserCreatedConfirmation> CreateUser(PersonalUser personalUser, string password)
        {
            if (!CheckUniqueUsername(personalUser.Username))
            {
                //Unique violation
                throw new UniqueValueViolationException("Username should be unique");
            }
            if (!CheckCity(personalUser.CityId))
            {
                throw new ForeignKeyConstraintViolationException("Foreign key constraint violated");
            }

            ////Adding to userdbcontext tables
            PersonalUserCreatedConfirmation userCreated = _personalUserRepository.CreateUser(personalUser);
            _personalUserRepository.SaveChanges();

            //Adding to identityuserdbcontext tables
            string username = string.Join("", personalUser.Username.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            var acc = new AccountInfo(username, personalUser.Email, userCreated.UserId);
            IdentityResult result = await _userManager.CreateAsync(acc, password);
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(acc, "Regular user").Wait();
            }
            else
            {
                _personalUserRepository.DeleteUser(userCreated.UserId);
                throw new ExectionException("Erorr trying to create user");
            }
            return userCreated;
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

        public async void DeleteUser(Guid userId)
        {
            _personalUserRepository.DeleteUser(userId);
            _personalUserRepository.SaveChanges();
            //Delete from identity table
            var acc = await _userManager.FindByIdAsync(userId.ToString());
            await _userManager.DeleteAsync(acc);
        }

        public PersonalUser GetUserByUserId(Guid userId)
        {
            return _personalUserRepository.GetUserByUserId(userId);
        }

        public List<PersonalUser> GetUsers(string city = null, string username = null)
        {
            return _personalUserRepository.GetUsers(city, username);
        }

        public async void UpdateUser(PersonalUser updatedUser, PersonalUser userWithId)
        {

            //TODO: Role can be changed only by admin, PATCH 
            //TODO: Cleaner code
            //TODO: Bad foreign keys, unique
            //TODO: Password change 
            if (!CheckUniqueUsername(updatedUser.Username))
            {
                //Unique violation
                throw new UniqueValueViolationException("Username should be unique");
            }

            updatedUser.RoleId = userWithId.RoleId;
            updatedUser.RoleId = userWithId.RoleId;
            updatedUser.Email = userWithId.Email;
            updatedUser.Role = _roleRepository.GetRoleByRoleId(userWithId.RoleId);
            City city = _cityRepository.GetCityByCityId(updatedUser.CityId);
            if (city != null)
            {
                throw new ForeignKeyConstraintViolationException("Foreign key constraint violated");
            }
            updatedUser.City = city;
            updatedUser.UserId = userWithId.UserId;
            _mapper.Map(updatedUser, userWithId);
            _personalUserRepository.SaveChanges();

            //Updating identity table
            AccountInfo acc = await _userManager.FindByIdAsync(userWithId.UserId.ToString());
            string username = string.Join("", updatedUser.Username.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            acc.UserName = username;
            await _userManager.UpdateAsync(acc);
        }
    }
}
