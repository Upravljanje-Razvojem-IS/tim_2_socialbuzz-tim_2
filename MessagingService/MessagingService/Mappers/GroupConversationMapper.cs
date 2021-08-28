using AutoMapper;
using MessagingService.Dtos;
using MessagingService.Entities;

namespace MessagingService.Mappers
{
    public class GroupConversationMapper : Profile
    {
        public GroupConversationMapper()
        {
            CreateMap<GroupConversation, GroupConversationReadDto>();
            CreateMap<GroupConversationCreateDto, GroupConversation>();
        }
    }
}
