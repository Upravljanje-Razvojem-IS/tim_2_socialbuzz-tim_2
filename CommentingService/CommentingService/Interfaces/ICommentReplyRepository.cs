using CommentingService.DTOs;
using System;
using System.Collections.Generic;

namespace CommentingService.Interfaces
{
    public interface ICommentReplyRepository
    {
        List<CommentReplyReadDto> Get();
        CommentReplyReadDto Get(Guid id);
        CommentReplyConfirmationDto Create(CommentReplyCreateDto dto);
        CommentReplyConfirmationDto Update(Guid id, CommentReplyCreateDto dto);
        void Delete(Guid id);
    }
}
