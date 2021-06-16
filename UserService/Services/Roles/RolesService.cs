using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Entities;

namespace UserService.Services.Roles
{
    public class RolesService : IRolesService
    {
        private readonly IRoleRepository _roleRepository;
        public RolesService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public RoleCreatedConfirmation CreateRole(Role role)
        {
            RoleCreatedConfirmation createdRole = _roleRepository.CreateRole(role);
            _roleRepository.SaveChanges();
            return createdRole;
        }

        public void DeleteRole(Guid roleId)
        {
            _roleRepository.DeleteRole(roleId);
            _roleRepository.SaveChanges();
        }

        public Role GetRoleByRoleId(Guid roleId)
        {
            return _roleRepository.GetRoleByRoleId(roleId);
        }

        public List<Role> GetRoles(string roleName)
        {
            return _roleRepository.GetRoles(roleName);
        }

        public void UpdateRole(Role role)
        {
            throw new NotImplementedException();
        }
    }
}
