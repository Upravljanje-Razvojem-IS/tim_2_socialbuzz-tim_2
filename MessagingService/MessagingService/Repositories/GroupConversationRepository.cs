using Microsoft.EntityFrameworkCore;
using MessagingService.Entities;
using MessagingService.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MessagingService.Repositories
{
    public class GroupConversationRepository : IGroupConversationRepository
    {
        private readonly AppDbContext _context;

        public GroupConversationRepository(AppDbContext context)
        {
            _context = context;
        }

        public GroupConversation Create(GroupConversation entity)
        {
            _context.GroupConversations.Add(entity);

            _context.SaveChanges();

            return entity;
        }

        public IEnumerable<GroupConversation> Get()
        {
            return _context.GroupConversations.Include(e => e.GroupMessages);
        }

        public GroupConversation Get(int id)
        {
            return _context.GroupConversations.Include(e => e.GroupMessages).FirstOrDefault(e => e.Id == id);
        }

        public GroupConversation Update(GroupConversation entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            _context.SaveChanges();

            return entity;
        }

        public GroupConversation Delete(int id)
        {
            GroupConversation entity = _context.GroupConversations.FirstOrDefault(e => e.Id == id);

            if (entity != null)
            {
                _context.GroupConversations.Remove(entity);
                _context.SaveChanges();
            }

            return entity;
        }
    }
}
