using System;

namespace UserWatchingService.Dtos.WatchingTypeDto
{
    public class WatchingTypeReadDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        public string Type { get; set; }
    }
}
