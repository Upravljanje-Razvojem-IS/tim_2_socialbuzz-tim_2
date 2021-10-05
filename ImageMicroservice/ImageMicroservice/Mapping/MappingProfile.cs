using AutoMapper;
using ImageMicroservice.Dto;
using ImageMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Post, PostDto>();
            CreateMap<PostDto, Post>();

            CreateMap<Image, ImageDto>();
            CreateMap<ImageDto, Image>();

            CreateMap<Image, ImageDownloadDto>();
            CreateMap<ImageDownloadDto, Image>();

            CreateMap<Image, ImageUploadDto>();
            CreateMap<ImageUploadDto, Image>();

        }
    }
}
