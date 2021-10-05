
using ImageMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Interfaces
{
    public interface IImageRepository : IRepositoryBase<Image>
    {
        List<Image> GetImagesByPostID(long postID);
        List<Image> GetAllForUser(long userID);
        void DeleteAllImagesForUser(long userID);
        void DeleteAllImagesForPost(long postID);
    }
}
