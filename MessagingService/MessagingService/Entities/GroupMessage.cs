using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessagingService.Entities
{
    public class GroupMessage
    {
        /// <summary>
        /// Unique identifier for <see cref="GroupMessage"/> entity.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Text of <see cref="GroupMessage"/>.
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string Text { get; set; }

        /// <summary>
        /// Date and time when <see cref="GroupMessage"/> was created.
        /// </summary>
        [Required]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Identifier of <see cref="User"/> that sent the <seealso cref="GroupMessage"/>.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int SenderId { get; set; }

        /// <summary>
        /// Identifier of the <see cref="Entities.GroupConversation"/> that the <seealso cref="GroupMessage"/> belongs to.
        /// </summary>
        [Required]
        [ForeignKey("GroupConversation")]
        [Range(1, int.MaxValue)]
        public int GroupConversationId { get; set; }

        /// <summary>
        /// Type object.
        /// </summary>
        public GroupConversation GroupConversation { get; set; }
    }
}
