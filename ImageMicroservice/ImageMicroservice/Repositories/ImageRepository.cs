using AutoMapper;
using ImageMicroservice.Interfaces;
using ImageMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        private readonly ImageMicroserviceDbContext _dbContext;
        

        public ImageRepository(ImageMicroserviceDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteAllImagesForPost(long postID)
        {
            List<Image> images = _dbContext.Images.Where(t => t.PostID == postID).ToList();

            foreach(Image image in images)
            {
                _dbContext.Images.Remove(image);
                _dbContext.SaveChanges();
            }
        }

        public void DeleteAllImagesForUser(long userID)
        {
            List<Image> images = _dbContext.Images.Where(t => t.UserID == userID).ToList();

            foreach (Image image in images)
            {
                _dbContext.Images.Remove(image);
                _dbContext.SaveChanges();
            }
        }

        public List<Image> GetAllForUser(long userID)
        {
            return _dbContext.Images.Where(t => t.UserID == userID).ToList();
        }

        public List<Image> GetImagesByPostID(long postID)
        {
            return _dbContext.Images.Where(t => t.PostID == postID).ToList();
        }
    }
}
