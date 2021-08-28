using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MessagingService.Dtos
{
    public class GroupConversationCreateDto
    {
        /// <summary>
        /// The name of the Conversation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Identifier of User that created the GroupConversation.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int CreatorId { get; set; }

        /// <summary>
        /// Identifiers of Users that participate in the Conversation.
        /// </summary>
        [Required]
        public List<int> ParticipantIdsList { get; set; }
    }
}
