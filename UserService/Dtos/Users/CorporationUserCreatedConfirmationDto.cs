using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos.Users
{
    public class CorporationUserCreatedConfirmationDto: UserCreatedConfirmationDto
    {
        /// <summary>
        /// Coropration name
        /// </summary>
        public String CorporationName { get; set; }

        /// <summary>
        /// Copropration PIB
        /// </summary>
        public String Pib { get; set; }

        /// <summary>
        /// City in which headquarters of corporation
        /// is located
        /// </summary>
        public String HeadquartersCity { get; set; }

        /// <summary>
        /// Address of the location in which headquarters of the corporation
        /// is located
        /// </summary>
        public String HeadquartersAddress { get; set; }
    }
}
