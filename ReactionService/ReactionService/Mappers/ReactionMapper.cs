using AutoMapper;
using ReactionService.Dtos;
using ReactionService.Entities;

namespace ReactionService.Mappers
{
    public class ReactionMapper : Profile
    {
        public ReactionMapper()
        {
            CreateMap<Reaction, ReactionReadDto>();
            CreateMap<ReactionCreateDto, Reaction>();
            CreateMap<ReactionUpdateDto, Reaction>();
        }
    }
}
