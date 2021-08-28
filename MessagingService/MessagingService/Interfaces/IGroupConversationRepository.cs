using MessagingService.Entities;
using System.Collections.Generic;

namespace MessagingService.Interfaces
{
    public interface IGroupConversationRepository
    {
        GroupConversation Create(GroupConversation entity);
        IEnumerable<GroupConversation> Get();
        GroupConversation Get(int id);
        GroupConversation Update(GroupConversation entity);
        GroupConversation Delete(int id);
    }
}
