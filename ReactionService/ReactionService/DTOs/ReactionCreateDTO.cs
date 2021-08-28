using System.ComponentModel.DataAnnotations;

namespace ReactionService.Dtos
{
    public class ReactionCreateDto
    {
        /// <summary>
        /// Identifier of the User that created Reaction.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        /// <summary>
        /// Identifier of the Post for which Reaction was created.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int PostId { get; set; }

        /// <summary>
        /// Identifier of the ReactionType of Reaction.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int ReactionTypeId { get; set; }
    }
}
