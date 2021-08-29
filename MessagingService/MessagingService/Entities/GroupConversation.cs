using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MessagingService.Entities
{
    public class GroupConversation
    {
        /// <summary>
        /// Unique identifier for <see cref="GroupConversation"/> entity.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The name of the <see cref="GroupConversation"/>.
        /// </summary>
        [MaxLength(128)]
        public string Name { get; set; }

        /// <summary>
        /// Identifier of <see cref="User"/> that created the <seealso cref="GroupConversation"/>.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int CreatorId { get; set; }

        /// <summary>
        /// Identifiers of <see cref="User"/>s that participate in the <seealso cref="Conversation"/>.
        /// </summary>
        [Required]
        public string ParticipantIds { get; set; }

        /// <summary>
        /// <see cref="GroupMessage"/>s that belong to the <seealso cref="GroupConversation"/>.
        /// </summary>
        public List<GroupMessage> GroupMessages { get; set; }

        public GroupConversation()
        {
            this.GroupMessages = new List<GroupMessage>();
        }
    }
}
