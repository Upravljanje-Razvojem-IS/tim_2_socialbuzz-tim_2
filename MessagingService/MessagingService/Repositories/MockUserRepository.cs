using MessagingService.Entities;
using MessagingService.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MessagingService.Repositories
{
    public class MockUserRepository : IUserRepository
    {
        private readonly List<User> _entities;

        public MockUserRepository()
        {
            _entities = new List<User>
            {
                new User { Id = 1, Username = "john123" },
                new User { Id = 2, Username = "nick123" },
                new User { Id = 3, Username = "mark123" },
                new User { Id = 4, Username = "mike123" },
                new User { Id = 5, Username = "will123" },
            };
        }

        public IEnumerable<User> Get()
        {
            return _entities;
        }

        public User Get(int id)
        {
            return _entities.FirstOrDefault(e => e.Id == id);
        }
    }
}
