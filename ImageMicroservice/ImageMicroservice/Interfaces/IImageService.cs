using ImageMicroservice.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Interfaces
{
    public interface IImageService
    {
        (string fileType, byte[] archiveData, string archiveName) GetAllImagesForPost(long postID);
        ImageDownloadDto GetImageByImageID(long imageID);
        (string fileType, byte[] archiveData, string archiveName) GetAllImagesForUser(long userID);
        ImageDto CreateImage(ImageUploadDto image);
        void DeleteImage(long id);
        void DeleteImagesByUserID(long id);
        void DeleteImagesByPostID(long id);
    }
}
