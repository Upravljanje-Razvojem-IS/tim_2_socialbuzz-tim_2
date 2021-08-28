using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommentingService.Entities
{
    public class CommentReply
    {
        /// <summary>
        /// Primary key of CommentReply
        /// </summary>
        [Key]
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
        [ForeignKey("Comment")]
        public Guid CommentId { get; set; }
        /// <summary>
        /// Comment
        /// </summary>
        public Comment Comment { get; set; }
        /// <summary>
        /// Id of user
        /// </summary>
        public Guid UserId { get; set; }
    }
}
