using AutoMapper;
using MessagingService.Dtos;
using MessagingService.Entities;

namespace MessagingService.Mappers
{
    public class MessageMapper : Profile
    {
        public MessageMapper()
        {
            CreateMap<Message, MessageReadDto>();
            CreateMap<MessageCreateDto, Message>();
        }
    }
}
