﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos
{
    public class PersonalUserDto: UserDto
    {
        public String FirstAndLastName { get; set; }
    }
}