using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos
{
    public class AccountInfoDto
    {
        public Guid Id { get; set; }
        public string Role { get; set; }
        public AccountInfoDto()
        {

        }
        
        public AccountInfoDto(string role, Guid id)
        {
            Id = id;
            Role = role;
        }

    }
}
