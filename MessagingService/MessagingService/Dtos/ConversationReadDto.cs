using System.Collections.Generic;

namespace MessagingService.Dtos
{
    public class ConversationReadDto
    {
        /// <summary>
        /// Unique identifier for Conversation entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the Conversation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Identifier of User that created the Conversation.
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// Identifier of User that participates in the Conversation.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Messages that belong to the Conversation.
        /// </summary>
        public List<MessageReadDto> Messages { get; set; }
    }
}
