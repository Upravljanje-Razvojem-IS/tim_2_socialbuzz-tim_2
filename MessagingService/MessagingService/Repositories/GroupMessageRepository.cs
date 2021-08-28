using Microsoft.EntityFrameworkCore;
using MessagingService.Entities;
using MessagingService.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MessagingService.Repositories
{
    public class GroupMessageRepository : IGroupMessageRepository
    {
        private readonly AppDbContext _context;

        public GroupMessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public GroupMessage Create(GroupMessage entity)
        {
            _context.GroupMessages.Add(entity);

            _context.SaveChanges();

            return entity;
        }

        public IEnumerable<GroupMessage> Get()
        {
            return _context.GroupMessages;
        }

        public GroupMessage Get(int id)
        {
            return _context.GroupMessages.FirstOrDefault(e => e.Id == id);
        }

        public GroupMessage Update(GroupMessage entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            _context.SaveChanges();

            return entity;
        }

        public GroupMessage Delete(int id)
        {
            GroupMessage entity = _context.GroupMessages.FirstOrDefault(e => e.Id == id);

            if (entity != null)
            {
                _context.GroupMessages.Remove(entity);
                _context.SaveChanges();
            }

            return entity;
        }
    }
}
