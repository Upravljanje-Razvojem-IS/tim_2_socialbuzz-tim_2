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

        public PersonalUserCreatedConfirmation CreateAdmin(PersonalUser user, string password)
        {
            if (!CheckUniqueUsername(user.Username, false, null))
            {
                //Unique violation
                throw new UniqueValueViolationException("Username should be unique");
            }
            if (!CkeckUniqueEmail(user.Email))
            {
                throw new UniqueValueViolationException("Email should be unique");
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
            IdentityResult result = _userManager.CreateAsync(acc, password).Result;
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

        public PersonalUserCreatedConfirmation CreateUser(PersonalUser personalUser, string password)
        {
            if (!CheckUniqueUsername(personalUser.Username, false, null))
            {
                //Unique violation
                throw new UniqueValueViolationException("Username should be unique");
            }
            if (!CkeckUniqueEmail(personalUser.Email))
            {
                throw new UniqueValueViolationException("Email should be unique");
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
            IdentityResult result = _userManager.CreateAsync(acc, password).Result;
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

        public void DeleteUser(Guid userId)
        {
            _personalUserRepository.DeleteUser(userId);
            _personalUserRepository.SaveChanges();
            //Delete from identity table
            var acc = _userManager.FindByIdAsync(userId.ToString()).Result;
            _userManager.DeleteAsync(acc).Wait();
        }

        public PersonalUser GetUserByUserId(Guid userId)
        {
            return _personalUserRepository.GetUserByUserId(userId);
        }

        public List<PersonalUser> GetUsers(string city = null, string username = null)
        {
            return _personalUserRepository.GetUsers(city, username);
        }

        public void UpdateUser(PersonalUser updatedUser, PersonalUser userWithId)
        {

            //TODO: Role can be changed only by admin, PATCH 
            //TODO: Cleaner code
            //TODO: Bad foreign keys, unique
            //TODO: Password change 
            if (!CheckUniqueUsername(updatedUser.Username, true, userWithId.UserId))
            {
                //Unique violation
                throw new UniqueValueViolationException("Username should be unique");
            }

            updatedUser.RoleId = userWithId.RoleId;
            updatedUser.RoleId = userWithId.RoleId;
            updatedUser.Email = userWithId.Email;
            updatedUser.Role = _roleRepository.GetRoleByRoleId(userWithId.RoleId);
            City city = _cityRepository.GetCityByCityId(updatedUser.CityId);
            if (city == null)
            {
                throw new ForeignKeyConstraintViolationException("Foreign key constraint violated");
            }
            updatedUser.City = city;
            updatedUser.UserId = userWithId.UserId;
            _mapper.Map(updatedUser, userWithId);
            _personalUserRepository.SaveChanges();

            //Updating identity table
            AccountInfo acc = _userManager.FindByIdAsync(userWithId.UserId.ToString()).Result;
            string username = string.Join("", updatedUser.Username.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            acc.UserName = username;
            _userManager.UpdateAsync(acc).Wait();
        }
        private bool CheckUniqueUsername(string username, bool forUpdate, Guid? userId)
        {
            var users = _corporationUserRepository.GetUsers(null, username);
            if (users != null && users.Count > 0)
            {
                if (!forUpdate || (forUpdate && users[0].UserId != userId))
                {
                    //Unique violation
                    return false;
                }
            }
            var persUsers = _personalUserRepository.GetUsers(null, username);
            if (persUsers != null && persUsers.Count > 0)
            {
                if (!forUpdate || (forUpdate && persUsers[0].UserId != userId))
                {
                    //Unique violation
                    return false;
                }

            }
            return true;
        }

        private bool CheckCity(Guid cityId)
        {
            return (_cityRepository.GetCityByCityId(cityId) != null);
        }
        private bool CkeckUniqueEmail(string email)
        {
            var users = _corporationUserRepository.GetUserWithEmail(email);
            if (users != null)
            {
                //Unique violation
                return false;
            }
            var persUsers = _personalUserRepository.GetUserWithEmail(email);
            if (persUsers != null)
            {
                //Unique violation
                return false;
            }
            return true;
        }

    }
}
