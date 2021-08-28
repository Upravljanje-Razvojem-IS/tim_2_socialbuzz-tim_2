using Microsoft.EntityFrameworkCore;
using ReactionService.Entities;
using ReactionService.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ReactionService.Repositories
{
    public class ReactionTypeRepository : IReactionTypeRepository
    {
        private readonly AppDbContext _context;

        public ReactionTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public ReactionType Create(ReactionType entity)
        {
            _context.ReactionTypes.Add(entity);

            _context.SaveChanges();

            return entity;
        }

        public IEnumerable<ReactionType> Get()
        {
            return _context.ReactionTypes;
        }

        public ReactionType Get(int id)
        {
            return _context.ReactionTypes.FirstOrDefault(e => e.Id == id);
        }

        public ReactionType Update(ReactionType entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            _context.SaveChanges();

            return entity;
        }

        public ReactionType Delete(int id)
        {
            ReactionType entity = _context.ReactionTypes.FirstOrDefault(e => e.Id == id);

            if (entity != null)
            {
                _context.ReactionTypes.Remove(entity);
                _context.SaveChanges();
            }

            return entity;
        }
    }
}
