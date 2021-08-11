using AutoMapper;
using BlockService.DTO;
using BlockService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockService.Profiles
{
    public class BlockingProfile: Profile
    {
        public BlockingProfile()
        {
            CreateMap<BlockModifyingDTO, Block>();

            CreateMap<BlockCreationDTO, Block>();

            CreateMap<Block, BlockDTO>();

            CreateMap<Block, Block>();
        }
           
    }
}
