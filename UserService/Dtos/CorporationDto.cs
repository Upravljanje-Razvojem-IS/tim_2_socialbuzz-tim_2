using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos
{
    public class CorporationDto: UserDto
    {
        public String CorporationName { get; set; }
        public String Pib { get; set; }
        public String HeadquartersCity { get; set; }
        public String HeadquartersAddress { get; set; }
    }
}
