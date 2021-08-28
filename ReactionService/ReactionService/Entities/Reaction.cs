using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactionService.Entities
{
    public class Reaction
    {
        /// <summary>
        /// Unique identifier for <see cref="Reaction"/> entity.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Date and time when <see cref="Reaction"/> was created.
        /// </summary>
        [Required]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Identifier of the <see cref="User"/> that created <seealso cref="Reaction"/>.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        /// <summary>
        /// Identifier of the <see cref="Post"/> for which <seealso cref="Reaction"/> was created.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int PostId { get; set; }

        /// <summary>
        /// Identifier of the <see cref="ReactionType"/> of <seealso cref="Reaction"/>.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        [ForeignKey("ReactionType")]
        public int ReactionTypeId { get; set; }

        /// <summary>
        /// Type object.
        /// </summary>
        public ReactionType ReactionType { get; set; }
    }
}
