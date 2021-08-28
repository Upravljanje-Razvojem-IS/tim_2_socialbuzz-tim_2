using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessagingService.Entities
{
    public class Message
    {
        /// <summary>
        /// Unique identifier for <see cref="Message"/> entity.
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// Text of <see cref="Message"/>.
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string Text { get; set; }

        /// <summary>
        /// Date and time when <see cref="Message"/> was created.
        /// </summary>
        [Required]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Status that indicates if <see cref="Message"/> has been read.
        /// </summary>
        [Required]
        public bool StatusRead { get; set; }

        /// <summary>
        /// Identifier of <see cref="User"/> that sent the <seealso cref="Message"/>.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int SenderId { get; set; }

        /// <summary>
        /// Identifier of <see cref="User"/> that received the <seealso cref="Message"/>.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int ReceiverId { get; set; }

        /// <summary>
        /// Identifier of the <see cref="Entities.Conversation"/> that the <seealso cref="Message"/> belongs to.
        /// </summary>
        [Required]
        [ForeignKey("Conversation")]
        [Range(1, int.MaxValue)]
        public int ConversationId { get; set; }

        /// <summary>
        /// Type object.
        /// </summary>
        public Conversation Conversation { get; set; }
    }
}
