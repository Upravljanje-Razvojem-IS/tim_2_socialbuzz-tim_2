using System;

namespace CommentingService.DTOs
{
    public class CommentCreateDto
    {
        /// <summary>
        /// Comment content
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Id of user who created comment
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Id of commented post
        /// </summary>
        public Guid PosId { get; set; }
    }
}
