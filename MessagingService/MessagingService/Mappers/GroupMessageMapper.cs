using AutoMapper;
using MessagingService.Dtos;
using MessagingService.Entities;

namespace MessagingService.Mappers
{
    public class GroupMessageMapper : Profile
    {
        public GroupMessageMapper()
        {
            CreateMap<GroupMessage, GroupMessageReadDto>();
            CreateMap<GroupMessageCreateDto, GroupMessage>();
        }
    }
}
