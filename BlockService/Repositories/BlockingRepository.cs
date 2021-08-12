using BlockService.Entities;
using BlockService.Entities.Mocks;
using BlockService.Repositories.FollowingMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockService.Repositories
{
    public class BlockingRepository : IBlockingRepository
    {
        private readonly ContextDB contextDB;
        private readonly IFollowingMockRepository followingMockRepository;

        public BlockingRepository(ContextDB contextDB, IFollowingMockRepository followingMockRepository)
        {
            this.contextDB = contextDB;
            this.followingMockRepository = followingMockRepository;
        }

        public Block Block(Block block)
        {
            var type = contextDB.Add(block);
            return type.Entity;
        }

        public Block GetBlockById(Guid blockID)
        {
            return contextDB.Block.FirstOrDefault(e => e.BlockID == blockID);
        }

        public bool CheckDoIFollowUser(int userID, int followingID)
        {
            return followingMockRepository.CheckDoIFollowUser(userID, followingID);
        }

        public List<Block> GetBlocksByUser(int userID)
        {
            var query = from block in contextDB.Block
                        where block.blockerID == userID
                        select block;

            return query.ToList();
        }

        public List<Block> GetBlocksForUser(int userID)
        {
            var query = from block in contextDB.Block
                        where block.blockedID == userID
                        select block;

            return query.ToList();
        }

        public void Unblock(int blockerID, int blockedID)
        {
            Block block = contextDB.Block.FirstOrDefault(block => block.blockedID == blockedID && block.blockerID == blockerID);
            contextDB.Remove(block);
        }

        public bool SaveChanges()
        {
            return contextDB.SaveChanges() > 0;
        }

        public bool CheckDidIAlreadyBlockUser(int userID, int blockedID)
        {
            var query = from block in contextDB.Block
                        select block;

            foreach (var v in query)
            {
                if (v.blockedID == userID && v.blockerID == blockedID) //Korisnik je mene blokirao
                {
                    return true;
                }
                else if (v.blockerID == userID && v.blockedID == blockedID) //Ja sam blokirala korisnika
                {
                    return true;
                }
            }

            return false;
        }

        public bool CheckDidIAlreadyUnblockUser(int userID, int blockedID)
        {
            var query = from block in contextDB.Block
                        select block;

            foreach (var v in query)
            {
                if (v.blockedID == userID && v.blockerID == blockedID) //Korisnik je mene blokirao
                {
                    return false;
                }
                else if (v.blockerID == userID && v.blockedID == blockedID) //Ja sam blokirala korisnika
                {
                    return false;
                }
            }

            return true;
        }

        public List<Block> GetAllBlocks()
        {
            return contextDB.Block.ToList();
        }

        public void UpdateBlock(Block block)
        {
            throw new NotImplementedException();
        }
    }
}
