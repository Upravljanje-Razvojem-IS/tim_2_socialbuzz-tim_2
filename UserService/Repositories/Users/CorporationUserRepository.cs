using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Data
{
    public class CorporationUserRepository : ICorporationUserRepository
    {
        private readonly UserDbContext context;
        private readonly IRoleRepository roleRepository;
        private readonly IMapper mapper;

        public CorporationUserRepository(UserDbContext context, IRoleRepository roleRepository, IMapper mapper)
        {
            this.context = context;
            this.roleRepository = roleRepository;
            this.mapper = mapper;
        }

        public CorporationUserCreatedConfirmation CreateUser(Corporation user)
        {
            var userRole = roleRepository.GetRoles("Regular user")[0];
            user.Role = userRole;
            context.Role.Attach(userRole);
            var createdUser = context.Add(user);
            return mapper.Map<CorporationUserCreatedConfirmation>(createdUser.Entity);
        }

        public void DeleteUser(Guid userId)
        {
            var user = GetUserByUserId(userId);
            context.Remove(user);
        }

        public Corporation GetUserByUserId(Guid userId)
        {
            return context.Corporation.Include(user => user.City).Include(user => user.Role).FirstOrDefault(e => e.UserId == userId);

        }

        public List<Corporation> GetUsers(string city = null, string username = null)
        {
            return context.Corporation.Include(user => user.City).Include(user => user.Role).
                Where(e => city == null || e.City.CityName == city).Where(e => username == null || e.Username.Equals(username)).
                ToList();
        }

        public List<Corporation> GetUsersWithCity(Guid id)
        {
            return context.Corporation.Include(user => user.City).Where(city => city.CityId == id).ToList();

        }

        public Corporation GetUserWithEmail(string email)
        {
            return context.Corporation.FirstOrDefault(user => user.Email == email);
        }

        public List<Corporation> GetUsersWithRole(Guid id)
        {
            return context.Corporation.Include(user => user.Role).Where(role => role.RoleId == id).ToList();
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateUser(Corporation user)
        {
            throw new NotImplementedException();
        }
    }
}
