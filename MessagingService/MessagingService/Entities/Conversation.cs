using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MessagingService.Entities
{
    public class Conversation
    {
        /// <summary>
        /// Unique identifier for <see cref="Conversation"/> entity.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The name of the <see cref="Conversation"/>.
        /// </summary>
        [MaxLength(128)]
        public string Name { get; set; }

        /// <summary>
        /// Identifier of <see cref="User"/> that created the <seealso cref="Conversation"/>.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int CreatorId { get; set; }

        /// <summary>
        /// Identifier of <see cref="User"/> that participates in the <seealso cref="Conversation"/>.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int ParticipantId { get; set; }

        /// <summary>
        /// <see cref="Message"/>s that belong to the <seealso cref="Conversation"/>.
        /// </summary>
        public List<Message> Messages { get; set; }

        public Conversation()
        {
            this.Messages = new List<Message>();
        }
    }
}
