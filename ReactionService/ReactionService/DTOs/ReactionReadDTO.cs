using System;

namespace ReactionService.Dtos
{
    public class ReactionReadDto
    {
        /// <summary>
        /// Unique identifier for Reaction entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date and time when Reaction was created.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Identifier of the User that created Reaction.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Identifier of the Post for which Reaction was created.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Identifier of the ReactionType of Reaction.
        /// </summary>
        public int ReactionTypeId { get; set; }
    }
}
