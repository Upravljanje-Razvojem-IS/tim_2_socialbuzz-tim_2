﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcelotGateway.Models
{
    public class AuthenticationResponse
    {

        public string Token { get; set; }
        public bool Succes { get; set; }
        public string Error { get; set; }
    }
}
