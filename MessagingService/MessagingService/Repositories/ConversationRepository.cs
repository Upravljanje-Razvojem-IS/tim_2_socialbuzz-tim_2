using Microsoft.EntityFrameworkCore;
using MessagingService.Entities;
using MessagingService.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MessagingService.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly AppDbContext _context;

        public ConversationRepository(AppDbContext context)
        {
            _context = context;
        }

        public Conversation Create(Conversation entity)
        {
            _context.Conversations.Add(entity);

            _context.SaveChanges();

            return entity;
        }

        public IEnumerable<Conversation> Get()
        {
            return _context.Conversations.Include(e => e.Messages);
        }

        public Conversation Get(int id)
        {
            return _context.Conversations.Include(e => e.Messages).FirstOrDefault(e => e.Id == id);
        }

        public Conversation Update(Conversation entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            _context.SaveChanges();

            return entity;
        }

        public Conversation Delete(int id)
        {
            Conversation entity = _context.Conversations.FirstOrDefault(e => e.Id == id);

            if (entity != null)
            {
                _context.Conversations.Remove(entity);
                _context.SaveChanges();
            }

            return entity;
        }
    }
}
