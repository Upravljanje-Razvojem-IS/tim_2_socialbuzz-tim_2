using ImageMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Interfaces
{
    public interface IPostRepository : IRepositoryBase<Post>
    {
        List<Post> GetByTitle(string title);
        List<Post> GetByUserID(long userID);
        List<Post> GetByCity(string title);
        void DeletePostsByUserID(long userID);
        Post UpdatePost(long postID, Post newPost);
    }
}
