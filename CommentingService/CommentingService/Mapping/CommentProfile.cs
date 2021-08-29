using AutoMapper;
using CommentingService.DTOs;
using CommentingService.Entities;

namespace CommentingService.Mapping
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentReadDto>();
            CreateMap<Comment, CommentConfirmationDto>();
        }
    }
}
