using AutoMapper;
using CommentingService.CustomException;
using CommentingService.Data;
using CommentingService.DTOs;
using CommentingService.Entities;
using CommentingService.Interfaces;
using CommentingService.LoggerMock;
using CommentingService.MockData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentingService.Repository
{
    public class CommentReplyRepository : ICommentReplyRepository
    {
        private readonly IMapper _mapper;
        private readonly DatabaseContext _context;
        private readonly FakeLogger _logger;

        public CommentReplyRepository(IMapper mapper, DatabaseContext context, FakeLogger logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }


        public CommentReplyConfirmationDto Create(CommentReplyCreateDto dto)
        {
            User user = MockedData.Users.FirstOrDefault(e => e.Id == dto.UserId);

            if (user == null)
                throw new BusinessException("User does not exit");

            CommentReply commentReply = new CommentReply()
            {
                Id = Guid.NewGuid(),
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = dto.UserId,
                CommentId = dto.CommentId
            };

            _context.CommentReplies.Add(commentReply);
            _context.SaveChanges();

            _logger.Log("Comment reply created");

            return _mapper.Map<CommentReplyConfirmationDto>(commentReply);
        }

        public void Delete(Guid id)
        {
            var commentReply = _context.CommentReplies.FirstOrDefault(e => e.Id == id);

            if (commentReply == null)
                throw new BusinessException("Comment reply with provided id does not exist");

            _context.CommentReplies.Remove(commentReply);
            _context.SaveChanges();

            _logger.Log("Comment reply deleted");
        }

        public List<CommentReplyReadDto> Get()
        {
            var list = _context.CommentReplies.ToList();

            _logger.Log("Comment reply list fetched");

            return _mapper.Map<List<CommentReplyReadDto>>(list);
        }

        public CommentReplyReadDto Get(Guid id)
        {
            var comment = _context.CommentReplies.FirstOrDefault(e => e.Id == id);

            _logger.Log("Comment reply fetched");

            return _mapper.Map<CommentReplyReadDto>(comment);
        }

        public CommentReplyConfirmationDto Update(Guid id, CommentReplyCreateDto dto)
        {
            var commentReply = _context.CommentReplies.FirstOrDefault(e => e.Id == id);

            if (commentReply == null)
                throw new BusinessException("Comment reply with provided id does not exist");

            User user = MockedData.Users.FirstOrDefault(e => e.Id == dto.UserId);

            if (user == null)
                throw new BusinessException("User does not exit");

            commentReply.Content = dto.Content;
            commentReply.UserId = dto.UserId;
            commentReply.CommentId = dto.CommentId;

            _context.SaveChanges();

            _logger.Log("Comment reply updated");

            return _mapper.Map<CommentReplyConfirmationDto>(commentReply);
        }
    }
}
