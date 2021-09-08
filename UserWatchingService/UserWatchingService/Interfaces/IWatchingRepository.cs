using System;
using System.Collections.Generic;
using UserWatchingService.Dtos.WatchingDto;

namespace UserWatchingService.Interfaces
{
    public interface IWatchingRepository
    {
        List<WatchingReadDto> Get();
        WatchingReadDto Get(Guid id);
        WatchingConfirmationDto Create(WatchingCreateDto dto);
        WatchingConfirmationDto Update(Guid id, WatchingCreateDto dto);
        void Delete(Guid id);
    }
}
