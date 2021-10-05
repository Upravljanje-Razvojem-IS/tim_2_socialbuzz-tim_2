using ImageMicroservice.Interfaces;
using ImageMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Repositories
{
    public class PostsRepository : Repository<Post>, IPostRepository 
    {
        private readonly ImageMicroserviceDbContext _dbContext;
        public PostsRepository(ImageMicroserviceDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeletePostsByUserID(long userID)
        {
            List<Post> posts = _dbContext.Posts.Where(t => t.UserID == userID).ToList();
            foreach(var post in posts)
            {
                _dbContext.Posts.Remove(post);
            }

            _dbContext.SaveChanges();
        }

        public List<Post> GetByCity(string title)
        {
            return _dbContext.Posts.Where(t => t.City.Contains(title)).ToList();
        }

        public List<Post> GetByTitle(string title)
        {
            return _dbContext.Posts.Where(t => t.PostTitle.Contains(title)).ToList();
        }

        public List<Post> GetByUserID(long userID)
        {
            return _dbContext.Posts.Where(t => t.UserID == userID).ToList();
        }

        public Post UpdatePost(long postID, Post newPost)
        {
            Post p = _dbContext.Posts.Find(postID);
            if(p != null)
            {
                p.Update(newPost);
                _dbContext.SaveChanges();
                return p;
            }
            return null;
        }
    }
}
