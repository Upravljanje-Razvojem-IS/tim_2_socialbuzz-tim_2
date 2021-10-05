using ImageMicroservice.Interfaces;
using ImageMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Repositories
{
    public class UsersRepository : Repository<User>, IUserRepository
    {
        private readonly ImageMicroserviceDbContext _dbContext;

        public UsersRepository(ImageMicroserviceDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public bool AreCredentialsValid(string username, string password)
        {
            return _dbContext.Users.Where(t => t.Username == username && t.Password == password).Count() > 0;
        }
    }
}
