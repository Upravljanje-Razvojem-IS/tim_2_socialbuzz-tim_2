using System;

namespace CommentingService.Entities
{
    public class User
    {
        /// <summary>
        /// Users Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }
    }
}
