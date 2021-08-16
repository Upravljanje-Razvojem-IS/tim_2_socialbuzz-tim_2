using BlockService.Entities;
using BlockService.Entities.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockService.Repositories
{
    public interface IBlockingRepository
    {
        List<Block> GetAllBlocks();
        Block Block(Block block); //create block
        Block GetBlockById(Guid blockID);
        bool CheckDoIFollowUser(int userID, int followingID);
        bool CheckDidIAlreadyBlockUser(int userID, int blockedID);
        bool CheckDidIAlreadyUnblockUser(int userID, int blockedID);
        void Unblock(int blockerID, int blockedID); //delete block
        void UpdateBlock(Block block);
        List<Block> GetBlocksByUser(int userID);
        List<Block> GetBlocksForUser(int userID);
        bool SaveChanges();
    }
}
