using System;

namespace MessagingService.Dtos
{
    public class GroupMessageReadDto
    {
        /// <summary>
        /// Unique identifier for GroupMessage entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Text of GroupMessage.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Date and time when GroupMessage was created.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Identifier of User that sent the GroupMessage.
        /// </summary>
        public int SenderId { get; set; }

        /// <summary>
        /// Identifier of the GroupConversation that the GroupMessage belongs to.
        /// </summary>
        public int GroupConversationId { get; set; }
    }
}
