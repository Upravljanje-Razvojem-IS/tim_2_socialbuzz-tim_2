using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Dto
{
    public class PostDto
    {
        public long PostID { get; set; }
        public string PostTitle { get; set; }
        public string City { get; set; }
        public string PostDescription { get; set; }
        public long UserID { get; set; }
    }
}
