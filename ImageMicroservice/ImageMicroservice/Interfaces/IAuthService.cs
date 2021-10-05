using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Interfaces
{
    public interface IAuthService
    {
        string Login(string username, string password);
    }
}
