using AutoMapper;
using ImageMicroservice.Dto;
using ImageMicroservice.Exceptions;
using ImageMicroservice.Interfaces;
using ImageMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Services
{
    public class PostsService : IPostService
    {
        private readonly IPostRepository _postRepo;
        private readonly IMapper _mapper;

        public PostsService(IPostRepository postRepo, IMapper mapper)
        {
            _postRepo = postRepo;
            _mapper = mapper;
        }

        public PostDto CreatePost(PostDto post)
        {
            Post postDomain = _mapper.Map<Post>(post);
            postDomain = _postRepo.Create(postDomain);
            return _mapper.Map<PostDto>(postDomain);
        }

        public void DeletePost(long id)
        {
            if (_postRepo.GetById(id) == null)
                throw new NotFound("Post not found");
            _postRepo.Delete(id);
        }

        public void DeletePostsByUserID(long userID)
        {
            _postRepo.DeletePostsByUserID(userID);
        }

        public List<PostDto> GetAllPostsFromWall()
        {
            return _mapper.Map<List<PostDto>>(_postRepo.GetAll());
        }

        public PostDto GetPostById(long id)
        {
            return _mapper.Map<PostDto>(_postRepo.GetById(id));
        }

        public List<PostDto> GetPostsByCity(string city)
        {
            return _mapper.Map<List<PostDto>>(_postRepo.GetByCity(city));
        }

        public List<PostDto> GetPostsByTitle(string title)
        {
            return _mapper.Map<List<PostDto>>(_postRepo.GetByTitle(title));
        }

        public List<PostDto> GetPostsByUserID(long userID)
        {
            return _mapper.Map<List<PostDto>>(_postRepo.GetByUserID(userID));
        }

        public PostDto UpdatePost(long postID, PostDto post)
        { 
            if (_postRepo.GetById(postID) == null)
                throw new NotFound("Post not found");
            Post postDomain = _mapper.Map<Post>(post);
            postDomain = _postRepo.UpdatePost(postID, postDomain);
            return _mapper.Map<PostDto>(postDomain);
        }
    }
}
