using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Dto
{
    public class ImageDownloadDto
    {
        public long ImageID { get; set; }
        public string ImagePath { get; set; }
        public long PostID { get; set; }
        public FileStream File { get; set; }
    }
}
