using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Models
{
    public class User
    {
        public long UserID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Telephone { get; set; }
        public bool IsActive { get; set; }
        public List<Note> Notes { get; set; }
    }
}
