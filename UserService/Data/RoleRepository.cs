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

        public RoleRepository(UserDbContext context)
        {
            this.context = context;
        }

        public RoleCreatedConfirmation CreateRole(Role role)
        {
            throw new NotImplementedException();
        }

        public void DeleteRole(Guid roleId)
        {
            throw new NotImplementedException();
        }

        public Role GetRoleByRoleId(Guid roleId)
        {
            return context.Role.FirstOrDefault(e => e.RoleId == roleId);

        }

        public List<Role> GetRoles()
        {
            return context.Role.ToList();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void UpdateRole(Role role)
        {
            throw new NotImplementedException();
        }
    }
}
