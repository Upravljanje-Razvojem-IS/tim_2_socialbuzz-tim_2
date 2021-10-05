using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Models
{
    public class Post
    {
        public long PostID { get; set; }
        public string PostTitle { get; set; }
        public string City { get; set; }
        public string PostDescription { get; set; }
        public List<Image> Images { get; set; }
        public User User { get; set; }
        public long UserID { get; set; }

        public void Update(Post newPost)
        {
            PostTitle = newPost.PostTitle;
            City = newPost.City;
            PostDescription = newPost.PostDescription;

        }
           
            
    }
}
