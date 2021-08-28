using ReactionService.Entities;
using System.Collections.Generic;

namespace ReactionService.Interfaces
{
    public interface IReactionTypeRepository
    {
        ReactionType Create(ReactionType entity);
        IEnumerable<ReactionType> Get();
        ReactionType Get(int id);
        ReactionType Update(ReactionType entity);
        ReactionType Delete(int id);
    }
}
