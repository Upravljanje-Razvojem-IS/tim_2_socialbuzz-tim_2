using ReactionService.Entities;
using System.Collections.Generic;

namespace ReactionService.Interfaces
{
    public interface IPostRepository
    {
        IEnumerable<Post> Get();
        Post Get(int id);
    }
}
