using System.ComponentModel.DataAnnotations;

namespace ReactionService.Entities
{
    public class ReactionType
    {
        /// <summary>
        /// Unique identifier for <see cref="ReactionType"/> entity.
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// Name of <see cref="ReactionType"/>.
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
    }
}
