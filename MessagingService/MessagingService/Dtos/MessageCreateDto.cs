using System.ComponentModel.DataAnnotations;

namespace MessagingService.Dtos
{
    public class MessageCreateDto
    {
        /// <summary>
        /// Text of Message.
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Identifier of User that sent the Message.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int SenderId { get; set; }

        /// <summary>
        /// Identifier of User that received the Message.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int ReceiverId { get; set; }

        /// <summary>
        /// Identifier of the Conversation that the Message belongs to.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int ConversationId { get; set; }
    }
}
