using AutoMapper;
using UserWatchingService.Dtos.WatchingDto;
using UserWatchingService.Entities;

namespace UserWatchingService.MapperProfiles
{
    public class WatchingMapper : Profile
    {
        public WatchingMapper()
        {
            CreateMap<Watching, WatchingReadDto>();
            CreateMap<Watching, WatchingConfirmationDto>();
        }
    }
}
