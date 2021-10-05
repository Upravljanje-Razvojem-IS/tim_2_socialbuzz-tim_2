using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Dto
{
    public class ImageDto
    {
        public long ImageID { get; set; }
        public string ImagePath { get; set; }
        public long UserID { get; set; }
        public long PostID { get; set; }
    }
}
