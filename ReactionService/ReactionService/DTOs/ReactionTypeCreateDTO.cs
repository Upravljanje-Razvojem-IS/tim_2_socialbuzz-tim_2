using System.ComponentModel.DataAnnotations;

namespace ReactionService.Dtos
{
    public class ReactionTypeCreateDto
    {
        /// <summary>
        /// Name of ReactionType.
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
    }
}
