using AutoMapper;
using ImageMicroservice.Dto;
using ImageMicroservice.Interfaces;
using ImageMicroservice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using ImageMicroservice.Exceptions;

namespace ImageMicroservice.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepo;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;

        public ImageService(IImageRepository imageRepo, IHostingEnvironment hostingEnvironment, IMapper mapper)
        {
            _imageRepo = imageRepo;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
        }

        public ImageDto CreateImage(ImageUploadDto image)
        {
            Image imageCreated = _mapper.Map<Image>(image);
            imageCreated.ImagePath = image.File.FileName;
            imageCreated = _imageRepo.Create(imageCreated);


            string filePath = Path.Combine(@$"Images/", imageCreated.ImagePath);
            new FileInfo(filePath).Directory?.Create();
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                image.File.CopyTo(stream);

            }

            return _mapper.Map<ImageDto>(imageCreated);
        }

        public void DeleteImage(long id)
        {
            Image image = _imageRepo.GetById(id);
            if (image == null)
                throw new NotFound("Image does not exist");
            _imageRepo.Delete(id);

            if (File.Exists(@$"Images/{image.ImagePath}"))
            {
                File.Delete(@$"Images/{image.ImagePath}");
            }
        }

        public void DeleteImagesByPostID(long id)
        {
            List<Image> images = _imageRepo.GetImagesByPostID(id);

            _imageRepo.DeleteAllImagesForPost(id);

            foreach(Image im in images)
            {
                if (File.Exists(@$"Images/{im.ImagePath}"))
                {
                    File.Delete(@$"Images/{im.ImagePath}");
                }
            }

        }

        public void DeleteImagesByUserID(long id)
        {
            List<Image> images = _imageRepo.GetAllForUser(id);

            _imageRepo.DeleteAllImagesForUser(id);

            foreach (Image im in images)
            {
                if (File.Exists(@$"Images/{im.ImagePath}"))
                {
                    File.Delete(@$"Images/{im.ImagePath}");
                }
            }
        }

        public (string fileType, byte[] archiveData, string archiveName) GetAllImagesForPost(long postID)
        {

            List<ImageDownloadDto> dto =_mapper.Map<List<ImageDownloadDto>>(_imageRepo.GetImagesByPostID(postID));
            var zipName = $"archive-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    dto.ForEach(file =>
                    {
                        var theFile = archive.CreateEntry(file.ImagePath);
                        using (var streamWriter = new StreamWriter(theFile.Open()))
                        {
                            using(FileStream stream = new FileStream(@$"Images/{file.ImagePath}", FileMode.Open))
                                streamWriter.Write(stream);
                        }

                    });
                }

                return ("application/zip", memoryStream.ToArray(), zipName);
            }
        }

        public (string fileType, byte[] archiveData, string archiveName) GetAllImagesForUser(long userID)
        {
            List<ImageDownloadDto> dto = _mapper.Map<List<ImageDownloadDto>>(_imageRepo.GetAllForUser(userID));
            var zipName = $"archive-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip"; ;

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    dto.ForEach(file =>
                    {
                        var theFile = archive.CreateEntry(file.ImagePath);
                        using (var streamWriter = new StreamWriter(theFile.Open()))
                        {
                            using (FileStream stream = new FileStream(@$"Images/{file.ImagePath}", FileMode.Open))
                                streamWriter.Write(stream);
                        }

                    });
                }

                return ("application/zip", memoryStream.ToArray(), zipName);
            }
        }

        public ImageDownloadDto GetImageByImageID(long imageID)
        {
            ImageDownloadDto dto = _mapper.Map<ImageDownloadDto>(_imageRepo.GetById(imageID));
            if (dto == null)
                throw new NotFound("Image does not exist");

            dto.File =  new FileStream(@$"Images/{dto.ImagePath}", FileMode.Open);
            return dto;
        }
    }
}
