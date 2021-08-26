using AutoMapper;
using PostService.Data;
using PostService.Data.BlockMockRepository;
using PostService.Data.UserMockRepository;
using PostService.Entities;
using PostService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PostService.Exceptions;
using PostService.Data.TypeOfPostRepository;

namespace PostService.Services
{
    public class PostsService : IPostService
    {
        private readonly IPostRepository postRepository;
        private readonly IUserMockRepository userMockRepository;
        private readonly IMapper autoMapper;
        private readonly IBlockMockRepository blockMockRepository;
        private readonly ITypeOfPostRepository typeRepository;
        public PostsService(IPostRepository postRepository, IUserMockRepository userMockRepository, IMapper autoMapper, IBlockMockRepository blockMockRepository, ITypeOfPostRepository typeRepository)
        {
            this.postRepository = postRepository;
            this.userMockRepository = userMockRepository;
            this.autoMapper = autoMapper;
            this.blockMockRepository = blockMockRepository;
            this.typeRepository = typeRepository;
        }

        public PostDto CreatePost(PostCreationDto post)
        {
            if(userMockRepository.GetUserByID(post.UserId) == null)
            {
                throw new _404Exception("User not found!");
            }
            if(!typeRepository.ContainsType(post.Type))
            {
                throw new _404Exception("Type not found!");
            }

            var newPost = autoMapper.Map<Post>(post);
            newPost.PostPublishingDateTime = DateTime.Now;
            newPost.LastModified = DateTime.Now;
            newPost.PostTypeId = typeRepository.GetIdByType(post.Type);
            try
            {
                var addedPost = postRepository.CreatePost(newPost);
                return autoMapper.Map<PostDto>(addedPost);
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeletePost(Guid PostId)
        {
            var toBeDeleted = postRepository.GetPostById(PostId);
            if(toBeDeleted == null)
            {
                throw new _404Exception("Post not found!");
            }
            try
            {
                postRepository.DeletePost(PostId);
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeletePostsByUserId(int UserId)
        {
            if(userMockRepository.GetUserByID(UserId) == null)
            {
                throw new _404Exception("User not found!");
            }
            postRepository.DeletePostsByUserId(UserId);
        }

        public PostDto GetPostById(Guid PostId)
        {
            if(postRepository.GetPostById(PostId) == null)
            {
                throw new _404Exception("Post not found!");
            }
            var fetchedPost = postRepository.GetPostById(PostId);
            return autoMapper.Map<PostDto>(fetchedPost);
        }

        public List<PostDto> GetPostsByUserId(int UserId)
        {
            if(userMockRepository.GetUserByID(UserId) == null)
            {
                throw new _404Exception("User not found!");
            }
            if(blockMockRepository.CheckIfBlocked(1,UserId))
            {
                throw new BlockException("User blocked!");
            }

            var fetchedPosts = postRepository.GetPostsByUserId(UserId);
            return autoMapper.Map<List<PostDto>>(fetchedPosts);
        }

        public List<PostDto> GetPosts()
        {
            var fetchedPosts = postRepository.GetPosts();
            return autoMapper.Map<List<PostDto>>(fetchedPosts);
        }


        public List<PostDto> GetPostsByCity(string City)
        {
            var fetchedPosts = postRepository.GetPostsByCity(1,City);
            if(fetchedPosts.Count() == 0)
            {
                throw new _404Exception("No posts found in city : " + City);
            }
           
            return autoMapper.Map<List<PostDto>>(fetchedPosts);
        }

        public List<PostDto> GetPostsByTitle(string PostTitle)
        {
            var fetchedPosts = postRepository.GetPostsByTitle(1,PostTitle);
            if (fetchedPosts == null)
            {
                throw new _404Exception("No posts found with title : " + PostTitle);
            }
            return autoMapper.Map<List<PostDto>>(fetchedPosts);
        }
        /*Prvo proveravamo blokiranje kako ne bismo slali zahtev za dzabe.*/
        public List<PostDto> GetPostsFromWall(int UserId, int SubjectId)
        {
            if(userMockRepository.GetUserByID(SubjectId) == null)
            {
                throw new _404Exception("User not found!");
            }

            if (blockMockRepository.CheckIfBlocked(UserId, SubjectId))      
            {
                throw new BlockException("User blocked!");
            }
            var fetchedPosts = postRepository.GetPostsFromWall(UserId, SubjectId);
            if (fetchedPosts == null)
            {
                throw new FollowingException("You are not following that user!");
            }
            return autoMapper.Map<List<PostDto>>(fetchedPosts);
        }

        public PostDto UpdatePost(PostModificationDto post)
        {

            if(postRepository.GetPostById(post.PostId) == null)
            {
                throw new _404Exception("Post with that Id does not exist!");
            }
            var oldPost = postRepository.GetPostById(post.PostId);
            var newPost = autoMapper.Map<Post>(post);

            if(oldPost.PostId != newPost.PostId)
            {
                throw new Exception("Changing of the post Id is not permitted!");
            }

            try
            {
                newPost.LastModified = DateTime.Now;
                newPost.UserId = oldPost.UserId;
                newPost.PostPublishingDateTime = oldPost.PostPublishingDateTime;
                autoMapper.Map(newPost, oldPost);
                postRepository.UpdatePost();
                return autoMapper.Map<PostDto>(newPost);
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
