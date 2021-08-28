using System;

namespace MessagingService.Dtos
{
    public class MessageReadDto
    {
        /// <summary>
        /// Unique identifier for Message entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Text of Message.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Date and time when Message was created.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Status that indicates if Message has been read.
        /// </summary>
        public bool StatusRead { get; set; }

        /// <summary>
        /// Identifier of User that sent the Message.
        /// </summary>
        public int SenderId { get; set; }

        /// <summary>
        /// Identifier of User that received the Message.
        /// </summary>
        public int ReceiverId { get; set; }

        /// <summary>
        /// Identifier of the Conversation that the Message belongs to.
        /// </summary>
        public int ConversationId { get; set; }
    }
}
