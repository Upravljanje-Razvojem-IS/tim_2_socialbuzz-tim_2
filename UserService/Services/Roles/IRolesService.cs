using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Services.Roles
{
    public interface IRolesService
    {
        List<Role> GetRoles(string roleName);
        Role GetRoleByRoleId(Guid roleId);
        RoleCreatedConfirmation CreateRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(Guid roleId);
    }
}
