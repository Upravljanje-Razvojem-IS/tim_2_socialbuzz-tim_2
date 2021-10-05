using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Dto
{
    public class ImageUploadDto
    {
        public long UserID { get; set; }
        public long PostID { get; set; }
        public IFormFile File { get; set; }
    }
}
