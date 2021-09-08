using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using UserWatchingService.CustomException;
using UserWatchingService.Data;
using UserWatchingService.Dtos.UserDto;
using UserWatchingService.Entities;
using UserWatchingService.Interfaces;
using UserWatchingService.Logger;

namespace UserWatchingService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataAccessLayer _db;
        private readonly IMapper _mapper;
        private readonly MockLogger _logger;

        public UserRepository(DataAccessLayer db, IMapper mapper, MockLogger logger)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
        }


        public UserConfirmationDto Create(UserCreateDto dto)
        {
            User newUser = new User()
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                Password = dto.Password,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsActive = dto.IsActive
            };

            _db.Users.Add(newUser);

            _db.SaveChanges();

            _logger.Log("Create User account!");

            return _mapper.Map<UserConfirmationDto>(newUser);
        }

        public List<UserReadDto> Get()
        {
            var list = _db.Users.ToList();

            _logger.Log("Get User accounts!");

            return _mapper.Map<List<UserReadDto>>(list);
        }

        public UserReadDto Get(Guid id)
        {
            var entity = _db.Users.FirstOrDefault(e => e.Id == id);

            _logger.Log("Get User account!");

            return _mapper.Map<UserReadDto>(entity);
        }

        public UserConfirmationDto Update(Guid id, UserCreateDto dto)
        {
            var user = _db.Users.FirstOrDefault(e => e.Id == id);

            if (user == null)
                throw new UserException("User does not exist");

            user.Username = dto.Username;
            user.Password = dto.Password;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;
            user.IsActive = dto.IsActive;

            _db.SaveChanges();

            _logger.Log("Update User account!");

            return _mapper.Map<UserConfirmationDto>(user);
        }

        public void Delete(Guid id)
        {
            var user = _db.Users.FirstOrDefault(e => e.Id == id);

            if (user == null)
                throw new UserException("User does not exist");

            _db.Users.Remove(user);

            _logger.Log("Delete User account!");

            _db.SaveChanges();
        }

    }
}
