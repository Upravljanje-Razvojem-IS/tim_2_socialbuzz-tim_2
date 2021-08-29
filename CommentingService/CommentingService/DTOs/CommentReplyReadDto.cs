using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentingService.DTOs
{
    public class CommentReplyReadDto
    {
        /// <summary>
        /// Primary key of CommentReply
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Replies comment
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Date and time of reply
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Id of comment
        /// </summary>
        public Guid CommentId { get; set; }
        /// <summary>
        /// Comment
        /// </summary>
        public CommentReadDto Comment { get; set; }
        /// <summary>
        /// Id of user
        /// </summary>
        public Guid UserId { get; set; }
    }
}
