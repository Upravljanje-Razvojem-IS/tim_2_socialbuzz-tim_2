using MessagingService.Entities;
using System.Collections.Generic;

namespace MessagingService.Interfaces
{
    public interface IGroupMessageRepository
    {
        GroupMessage Create(GroupMessage entity);
        IEnumerable<GroupMessage> Get();
        GroupMessage Get(int id);
        GroupMessage Update(GroupMessage entity);
        GroupMessage Delete(int id);
    }
}
