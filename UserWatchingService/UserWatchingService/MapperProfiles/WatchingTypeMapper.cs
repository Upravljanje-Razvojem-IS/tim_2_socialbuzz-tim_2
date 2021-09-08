using AutoMapper;
using UserWatchingService.Dtos.WatchingTypeDto;
using UserWatchingService.Entities;

namespace UserWatchingService.MapperProfiles
{
    public class WatchingTypeMapper : Profile
    {
        public WatchingTypeMapper()
        {
            CreateMap<WatchingType, WatchingTypeReadDto>();
            CreateMap<WatchingType, WatchingTypeConfirmationDto>();
        }
    }
}
