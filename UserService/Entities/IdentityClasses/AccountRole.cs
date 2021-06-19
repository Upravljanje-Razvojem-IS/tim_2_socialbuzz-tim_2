using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{

    /// <summary>
    /// Entity which models roles in the system,
    /// which are used for authorization 
    /// </summary>
    public class AccountRole : IdentityRole<Guid>
    {
        /// <summary>
        /// Descripton of a given role
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }

        public AccountRole() : base()
        {
            Description = "no description";
        }

        public AccountRole(string description, string roleName) : base(roleName)
        {
            Description = description;
        }

        public AccountRole(Guid id, string description, string roleName) : base(roleName)
        {
            base.Id = id;
            Description = description;
        }
    }
}
