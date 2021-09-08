using System;

namespace UserWatchingService.Dtos.WatchingDto
{
    public class WatchingConfirmationDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Watcher id 
        /// </summary>
        public Guid WatcherId { get; set; }
        /// <summary>
        /// Watched id
        /// </summary>
        public Guid WatchedId { get; set; }
    }
}
