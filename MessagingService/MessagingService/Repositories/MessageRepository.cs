using Microsoft.EntityFrameworkCore;
using MessagingService.Entities;
using MessagingService.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MessagingService.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public Message Create(Message entity)
        {
            _context.Messages.Add(entity);

            _context.SaveChanges();

            return entity;
        }

        public IEnumerable<Message> Get()
        {
            return _context.Messages;
        }

        public Message Get(int id)
        {
            return _context.Messages.FirstOrDefault(e => e.Id == id);
        }

        public Message Update(Message entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            _context.SaveChanges();

            return entity;
        }

        public Message Delete(int id)
        {
            Message entity = _context.Messages.FirstOrDefault(e => e.Id == id);

            if (entity != null)
            {
                _context.Messages.Remove(entity);
                _context.SaveChanges();
            }

            return entity;
        }
    }
}
