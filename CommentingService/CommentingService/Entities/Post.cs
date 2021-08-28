using System;

namespace CommentingService.Entities
{
    public class Post
    {
        /// <summary>
        /// Posts Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
    }
}
