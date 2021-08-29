using AutoMapper;
using CommentingService.DTOs;
using CommentingService.Entities;

namespace CommentingService.Mapping
{
    public class CommentReplyProfile : Profile
    {
        public CommentReplyProfile()
        {
            CreateMap<CommentReply, CommentReplyReadDto>();
            CreateMap<CommentReply, CommentReplyConfirmationDto>();
        }
    }
}
