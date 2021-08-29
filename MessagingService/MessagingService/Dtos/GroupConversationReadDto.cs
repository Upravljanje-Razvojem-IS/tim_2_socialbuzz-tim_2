using System.Collections.Generic;

namespace MessagingService.Dtos
{
    public class GroupConversationReadDto
    {
        /// <summary>
        /// Unique identifier for GroupConversation entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the Conversation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Identifier of User that created the GroupConversation.
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// Identifiers of Users that participate in the Conversation.
        /// </summary>
        public List<int> ParticipantIdsList { get; set; }

        /// <summary>
        /// Messages that belong to the GroupConversation.
        /// </summary>
        public List<GroupMessageReadDto> GroupMessages { get; set; }
    }
}
