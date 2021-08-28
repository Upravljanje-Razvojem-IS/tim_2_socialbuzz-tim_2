using System.ComponentModel.DataAnnotations;

namespace MessagingService.Dtos
{
    public class GroupMessageCreateDto
    {
        /// <summary>
        /// Text of GroupMessage.
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Identifier of User that sent the GroupMessage.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int SenderId { get; set; }

        /// <summary>
        /// Identifier of the GroupConversation that the GroupMessage belongs to.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int GroupConversationId { get; set; }
    }
}
