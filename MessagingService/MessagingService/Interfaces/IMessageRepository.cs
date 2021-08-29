using MessagingService.Entities;
using System.Collections.Generic;

namespace MessagingService.Interfaces
{
    public interface IMessageRepository
    {
        Message Create(Message entity);
        IEnumerable<Message> Get();
        Message Get(int id);
        Message Update(Message entity);
        Message Delete(int id);
    }
}
