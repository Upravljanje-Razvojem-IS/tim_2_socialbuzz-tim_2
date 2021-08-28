using ReactionService.Entities;
using ReactionService.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ReactionService.Repositories
{
    public class MockPostRepository : IPostRepository
    {
        private List<Post> _entities;

        public MockPostRepository()
        {
            _entities = new List<Post>
            {
                new Post { Id = 1, Title = "My first post.", Description = "This is my first post.", UserId = 1 },
                new Post { Id = 2, Title = "My second post.", Description = "This is my second post.", UserId = 2 },
                new Post { Id = 3, Title = "My third post.", Description = "This is my third post.", UserId = 3 },
                new Post { Id = 4, Title = "My fourth post.", Description = "This is my fourth post.", UserId = 1 },
                new Post { Id = 5, Title = "My fifth post.", Description = "This is my fifth post.", UserId = 1 },
            };
        }

        public IEnumerable<Post> Get()
        {
            return _entities;
        }

        public Post Get(int id)
        {
            return _entities.FirstOrDefault(e => e.Id == id);
        }
    }
}
