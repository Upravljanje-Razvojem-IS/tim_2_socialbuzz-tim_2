using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Models
{
    public class Note
    {
        public long NoteID { get; set; }
        public string NoteText { get; set; }
        public DateTime? NoteCreated { get; set; }
        public DateTime? NoteLastModified { get; set; }
        public bool NoteFavorited { get; set; }
        public long UserID { get; set; }
        public User User { get; set; }
    }
}
