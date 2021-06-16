using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Data
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UserDbContext context;
        private readonly IMapper mapper;

        public RoleRepository(UserDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public RoleCreatedConfirmation CreateRole(Role role)
        {
            var createdRole = context.Add(role);
            return mapper.Map<RoleCreatedConfirmation>(createdRole.Entity);
        }

        public void DeleteRole(Guid roleId)
        {
            var role = GetRoleByRoleId(roleId);
            context.Remove(role);
        }

        public Role GetRoleByRoleId(Guid roleId)
        {
            return context.Role.FirstOrDefault(e => e.RoleId == roleId);

        }

        public List<Role> GetRoles(string roleName)
        {
            return context.Role.Where(c => roleName == null || c.RoleName == roleName).ToList();
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateRole(Role role)
        {
            throw new NotImplementedException();
        }
    }
}
