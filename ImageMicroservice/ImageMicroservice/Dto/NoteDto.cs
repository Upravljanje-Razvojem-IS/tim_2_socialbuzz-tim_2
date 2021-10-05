using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Dto
{
    public class NoteDto
    {
        public long NoteID { get; set; }
        public string NoteText { get; set; }
        public DateTime? NoteCreated { get; set; }
        public DateTime? NoteLastModified { get; set; }
        public bool NoteFavorited { get; set; }
        public UserDto User { get; set; }
    }
}
