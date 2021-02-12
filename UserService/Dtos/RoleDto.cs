using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos
{
    /// <summary>
    /// DTO class which models roles in the system,
    /// which are used for authorization 
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        /// Unique identifier for the role
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Role name
        /// </summary>
        public String RoleName { get; set; }
    }
}
