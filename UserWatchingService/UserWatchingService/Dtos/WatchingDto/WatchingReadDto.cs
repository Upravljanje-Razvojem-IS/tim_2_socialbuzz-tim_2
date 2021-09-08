using System;
using UserWatchingService.Entities;

namespace UserWatchingService.Dtos.WatchingDto
{
    public class WatchingReadDto
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
