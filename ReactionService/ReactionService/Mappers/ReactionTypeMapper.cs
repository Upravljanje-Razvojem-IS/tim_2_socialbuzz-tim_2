using AutoMapper;
using ReactionService.Dtos;
using ReactionService.Entities;

namespace ReactionService.Mappers
{
    public class ReactionTypeMapper : Profile
    {
        public ReactionTypeMapper()
        {
            CreateMap<ReactionType, ReactionTypeReadDto>();
            CreateMap<ReactionTypeCreateDto, ReactionType>();
            CreateMap<ReactionTypeUpdateDto, ReactionType>();
        }
    }
}
