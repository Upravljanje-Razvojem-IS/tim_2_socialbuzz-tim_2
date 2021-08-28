using ReactionService.Entities;
using System.Collections.Generic;

namespace ReactionService.Interfaces
{
    public interface IReactionRepository
    {
        Reaction Create(Reaction entity);
        IEnumerable<Reaction> Get();
        Reaction Get(int id);
        Reaction Update(Reaction entity);
        Reaction Delete(int id);
    }
}
