using MessagingService.Entities;
using System.Collections.Generic;

namespace MessagingService.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> Get();
        User Get(int id);
    }
}
