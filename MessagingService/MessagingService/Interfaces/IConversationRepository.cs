using MessagingService.Entities;
using System.Collections.Generic;

namespace MessagingService.Interfaces
{
    public interface IConversationRepository
    {
        Conversation Create(Conversation entity);
        IEnumerable<Conversation> Get();
        Conversation Get(int id);
        Conversation Update(Conversation entity);
        Conversation Delete(int id);
    }
}
