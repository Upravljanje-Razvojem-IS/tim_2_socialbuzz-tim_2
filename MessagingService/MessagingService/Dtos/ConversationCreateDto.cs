using System;
using System.ComponentModel.DataAnnotations;

namespace MessagingService.Dtos
{
    public class ConversationCreateDto
    {
        /// <summary>
        /// The name of the Conversation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Identifier of User that created the Conversation.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int CreatorId { get; set; }

        /// <summary>
        /// Identifier of User that participates in the Conversation.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int ParticipantId { get; set; }
    }
}
