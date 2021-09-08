using System;
using System.ComponentModel.DataAnnotations;

namespace UserWatchingService.Entities
{
    public class WatchingType
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        [Required]
        public string Type { get; set; }
    }
}
