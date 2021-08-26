using PostService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Services
{
    public interface IPostService
    {
        List<PostDto> GetPosts();

        List<PostDto> GetPostsFromWall(int UserId, int SubjectId);

        PostDto GetPostById(Guid PostId);

        List<PostDto> GetPostsByUserId(int UserId);

        List<PostDto> GetPostsByTitle(string PostTitle);

        List<PostDto> GetPostsByCity(string City);

        public PostDto CreatePost(PostCreationDto post);

        public PostDto UpdatePost(PostModificationDto post);

        public void DeletePost(Guid PostId);

        public void DeletePostsByUserId(int UserId);
    }
}
