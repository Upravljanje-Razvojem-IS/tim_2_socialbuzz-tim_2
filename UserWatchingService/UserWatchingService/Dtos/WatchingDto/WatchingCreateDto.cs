using System;

namespace UserWatchingService.Dtos.WatchingDto
{
    public class WatchingCreateDto
    {
        /// <summary>
        /// Watcher id
        /// </summary>
        public Guid WatcherId { get; set; }
        /// <summary>
        /// watched id
        /// </summary>
        public Guid WatchedId { get; set; }
    }
}
