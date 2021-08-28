using Microsoft.EntityFrameworkCore;
using ReactionService.Entities;
using ReactionService.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ReactionService.Repositories
{
    public class ReactionRepository : IReactionRepository
    {
        private readonly AppDbContext _context;

        public ReactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public Reaction Create(Reaction entity)
        {
            _context.Reactions.Add(entity);

            _context.SaveChanges();

            return entity;
        }

        public IEnumerable<Reaction> Get()
        {
            return _context.Reactions;
        }

        public Reaction Get(int id)
        {
            return _context.Reactions.FirstOrDefault(e => e.Id == id);
        }

        public Reaction Update(Reaction entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            _context.SaveChanges();

            return entity;
        }

        public Reaction Delete(int id)
        {
            Reaction entity = _context.Reactions.FirstOrDefault(e => e.Id == id);

            if (entity != null)
            {
                _context.Reactions.Remove(entity);
                _context.SaveChanges();
            }

            return entity;
        }
    }
}
