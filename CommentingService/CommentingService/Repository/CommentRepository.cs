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

namespace CommentingService.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly FakeLogger _logger;

        public CommentRepository(IMapper mapper, DatabaseContext context, FakeLogger logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        public CommentConfirmationDto Create(CommentCreateDto dto)
        {
            User user = MockedData.Users.FirstOrDefault(e => e.Id == dto.UserId);

            if (user == null)
                throw new BusinessException("User does not exit");

            Post post = MockedData.Posts.FirstOrDefault(e => e.Id == dto.UserId);

            if (post == null)
                throw new BusinessException("Post does not exit");

            Comment comment = new Comment()
            {
                Id = Guid.NewGuid(),
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = dto.UserId,
                PosId = dto.PosId
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            _logger.Log("Comment created");

            return _mapper.Map<CommentConfirmationDto>(comment);
        }

        public void Delete(Guid id)
        {
            var comment = _context.Comments.FirstOrDefault(e => e.Id == id);

            if (comment == null)
                throw new BusinessException("Comment with provided id does not exist");

            _context.Comments.Remove(comment);
            _context.SaveChanges();

            _logger.Log("Comment deleted");

        }

        public List<CommentReadDto> Get()
        {
            var list = _context.Comments.ToList();

            _logger.Log("Comment list fetched");

            return _mapper.Map<List<CommentReadDto>>(list);
        }

        public CommentReadDto Get(Guid id)
        {
            var comment = _context.Comments.FirstOrDefault(e => e.Id == id);

            _logger.Log("Comment fetched");

            return _mapper.Map<CommentReadDto>(comment);
        }

        public CommentConfirmationDto Update(Guid id, CommentCreateDto dto)
        {
            var comment = _context.Comments.FirstOrDefault(e => e.Id == id);

            if (comment == null)
                throw new BusinessException("Comment with provided id does not exist");

            User user = MockedData.Users.FirstOrDefault(e => e.Id == dto.UserId);

            if (user == null)
                throw new BusinessException("User does not exit");

            Post post = MockedData.Posts.FirstOrDefault(e => e.Id == dto.UserId);

            if (post == null)
                throw new BusinessException("Post does not exit");

            comment.Content = dto.Content;
            comment.UserId = dto.UserId;
            comment.PosId = dto.PosId;

            _context.SaveChanges();

            _logger.Log("Comment updated");

            return _mapper.Map<CommentConfirmationDto>(comment);
        }
    }
}
