using System;
using System.Collections.Generic;
using UserWatchingService.Dtos.UserDto;

namespace UserWatchingService.Interfaces
{
    public interface IUserRepository
    {
        List<UserReadDto> Get();
        UserReadDto Get(Guid id);
        UserConfirmationDto Create(UserCreateDto dto);
        UserConfirmationDto Update(Guid id, UserCreateDto dto);
        void Delete(Guid id);
    }
}
