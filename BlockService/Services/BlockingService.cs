using AutoMapper;
using BlockService.DTO;
using BlockService.Entities;
using BlockService.Exceptions;
using BlockService.Repositories;
using BlockService.Repositories.FollowingMock;
using BlockService.Repositories.UserMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockService.Services
{
    public class BlockingService : IBlockingService
    {
        private readonly IBlockingRepository _blockingRepository;
        private readonly IUserMockRepository _userMockRepository;
        private readonly IMapper mapper;

        public BlockingService(IBlockingRepository blockingRepository,
                               IMapper mapper, IUserMockRepository userRepository)
        {
            _blockingRepository = blockingRepository;
            _userMockRepository = userRepository;
            this.mapper = mapper;
        }

        public BlockDTO Block(BlockCreationDTO block, int blockerID, int blockedID)
        {
            if (_userMockRepository.GetUserByID(blockedID) == null)
            {
                throw new NotFoundException("User with that ID does not exist!");
            }

            if (_userMockRepository.GetUserByID(blockerID) == null)
            {
                throw new NotFoundException("User with that ID does not exist!");
            }

            if (blockerID == blockedID)
            {
                throw new ErrorOccurException("You can not block yourself!");
            }

            if (!_blockingRepository.CheckDoIFollowUser(blockerID, blockedID))
            {
                throw new FollowingException("You dont follow user with that ID, so you can not block him!");
            }

            if (_blockingRepository.CheckDidIAlreadyBlockUser(blockerID, blockedID))
            {
                throw new BlockingException("You already blocked this user, you are not following him!");
            }

            Block type = mapper.Map<Block>(block);
            type.BlockDate = DateTime.Now;

            try
            {
                var created = _blockingRepository.Block(type);
                _blockingRepository.SaveChanges();
                return mapper.Map<BlockDTO>(created);
            }
            catch (Exception ex)
            {
                throw new ErrorOccurException(ex.Message);
            }
        }

        public List<BlockDTO> GetAllBlocks()
        {
            var blocks = _blockingRepository.GetAllBlocks();
            return mapper.Map<List<BlockDTO>>(blocks);
        }

        public BlockDTO GetBlockById(Guid blockID)
        {
            var type = _blockingRepository.GetBlockById(blockID);

            if (type == null)
            {
                throw new NotFoundException("Block with that ID does not exist");
            }

            return mapper.Map<BlockDTO>(type);
        }

        public List<BlockDTO> GetBlocksByUser(int userID)
        {
            var userId = _userMockRepository.GetUserByID(userID);

            if (userId == null)
            {
                throw new NotFoundException("There is no user with that ID ...");
            }

            var blocks = _blockingRepository.GetBlocksByUser(userID);

            if (blocks == null)
            {
                throw new NotFoundException("This user have not yet blocked any other user ...");
            }

            return mapper.Map<List<BlockDTO>>(blocks);
        }

        public List<BlockDTO> GetBlocksForUser(int userID)
        {
            var userId = _userMockRepository.GetUserByID(userID);

            if (userId == null)
            {
                throw new NotFoundException("There is no user with that ID ...");
            }

            var blocks = _blockingRepository.GetBlocksForUser(userID);

            if (blocks == null)
            {
                throw new NotFoundException("This user is not blocked by any other user yet ...");
            }

            return mapper.Map<List<BlockDTO>>(blocks);
        }

        public void Unblock(int blockerID, int blockedID)
        {
            if (_userMockRepository.GetUserByID(blockedID) == null)
            {
                throw new NotFoundException("User with that ID does not exist!");
            }

            if (_userMockRepository.GetUserByID(blockerID) == null)
            {
                throw new NotFoundException("User with that ID does not exist!");
            }

            if (!_blockingRepository.CheckDoIFollowUser(blockerID, blockedID))
            {
                throw new FollowingException("You dont follow user with that ID, so you can not unblock him!");
            }

            if (_blockingRepository.CheckDidIAlreadyUnblockUser(blockerID, blockedID))
            {
                throw new BlockingException("You already unblocked this user, you can not do it again!");
            }

            try
            {
                _blockingRepository.Unblock(blockerID, blockedID);
                _blockingRepository.SaveChanges();
            }

            catch (Exception ex)
            {
                throw new ErrorOccurException("Error deleting reaction: " + ex.Message);
            }
        }

        public void UpdateBlock(BlockModifyingDTO block, Guid BlockID)
        {
            var oldType = _blockingRepository.GetBlockById(BlockID);

            if (oldType == null)
            {
                throw new NotFoundException("There is no block with that ID");
            }

            var newType = mapper.Map<Block>(block);
            newType.BlockID = BlockID;
            newType.BlockDate = DateTime.Now;

            

            if (!_blockingRepository.CheckDoIFollowUser(newType.blockerID, newType.blockedID))
            {
                throw new FollowingException("You dont follow user with that ID, so you can not block him!");
            }

            if (_blockingRepository.CheckDidIAlreadyBlockUser(newType.blockerID, newType.blockedID))
            {
                throw new BlockingException("You already blocked this user, you are not following him!");
            }

            try
            {
                mapper.Map(newType, oldType);
                _blockingRepository.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new ErrorOccurException("Error updating block: " + ex.Message);

            }
        }
    }
}
