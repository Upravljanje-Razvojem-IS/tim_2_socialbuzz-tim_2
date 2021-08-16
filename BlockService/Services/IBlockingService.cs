using BlockService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockService.Services
{
    public interface IBlockingService
    {
        List<BlockDTO> GetAllBlocks();
        BlockDTO Block(BlockCreationDTO block, int blockerID, int blockedID); 
        BlockDTO GetBlockById(Guid blockID);
        void Unblock(int blockerID, int blockedID);
        void UpdateBlock(BlockModifyingDTO block, Guid BlockID);
        List<BlockDTO> GetBlocksByUser(int userID);
        List<BlockDTO> GetBlocksForUser(int userID);
    }
}
