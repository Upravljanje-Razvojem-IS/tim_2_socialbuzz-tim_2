using ImageMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        bool AreCredentialsValid(string username, string password);
    }
}
