using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UserWatchingService.Entities
{
    public class User
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// IsActive 
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Number
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Username
        /// </summary>
        [Required]
        public string Username { get; set; }

        public List<Watching> WatchingList { get; set; }
        public List<Watching> WatcherList { get; set; }
    }
}
