using AutoMapper;
using MessagingService.Dtos;
using MessagingService.Entities;

namespace MessagingService.Mappers
{
    public class ConversationMapper : Profile
    {
        public ConversationMapper()
        {
            CreateMap<Conversation, ConversationReadDto>();
            CreateMap<ConversationCreateDto, Conversation>();
        }
    }
}
