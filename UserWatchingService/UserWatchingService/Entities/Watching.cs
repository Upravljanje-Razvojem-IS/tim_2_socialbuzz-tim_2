using System;
using System.ComponentModel.DataAnnotations;

namespace UserWatchingService.Entities
{
    public class Watching
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// Watcher Id
        /// </summary>
        public Guid WatcherId { get; set; }
        /// <summary>
        /// Watcher object
        /// </summary>
        public User Watcher { get; set; }
        /// <summary>
        /// WatchedId
        /// </summary>
        public Guid WatchedId { get; set; }
        /// <summary>
        /// Watched object
        /// </summary>
        public User Watched { get; set; }
    }
}
