using ImageMicroservice.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Interfaces
{
    public interface IPostService
    {
        List<PostDto> GetAllPostsFromWall();
        PostDto GetPostById(long id);
        List<PostDto> GetPostsByTitle(string title);
        List<PostDto> GetPostsByUserID(long userID);
        List<PostDto> GetPostsByCity(string city);
        PostDto CreatePost(PostDto post);
        PostDto UpdatePost(long postID, PostDto post);
        void DeletePost(long id);
        void DeletePostsByUserID(long userID);
    }
}
