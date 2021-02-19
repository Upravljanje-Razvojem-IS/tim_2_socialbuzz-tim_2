using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Data
{
    interface IRoleRepository
    {
        List<Role> GetRoles();
        Role GetRoleByRoleId(Guid roleId);
        RoleCreatedConfirmation CreateUser(Role role);
        void UpdateRole(Role role);
        void DeleteRole(Guid roleId);
        bool SaveChanges();
    }
}
