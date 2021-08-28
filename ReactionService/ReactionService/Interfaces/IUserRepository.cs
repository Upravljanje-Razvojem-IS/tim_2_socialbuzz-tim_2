using ReactionService.Entities;
using System.Collections.Generic;

namespace ReactionService.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> Get();
        User Get(int id);
    }
}
