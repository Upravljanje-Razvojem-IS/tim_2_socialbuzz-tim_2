using System;
using System.Collections.Generic;
using UserWatchingService.Dtos.WatchingTypeDto;

namespace UserWatchingService.Interfaces
{
    public interface IWatchingTypeRepository
    {
        List<WatchingTypeReadDto> Get();
        WatchingTypeReadDto Get(Guid id);
        WatchingTypeConfirmationDto Create(WatchingTypeCreateDto dto);
        WatchingTypeConfirmationDto Update(Guid id, WatchingTypeCreateDto dto);
        void Delete(Guid id);
    }
}
