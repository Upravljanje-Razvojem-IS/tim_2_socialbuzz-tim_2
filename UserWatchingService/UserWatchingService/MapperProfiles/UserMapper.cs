using AutoMapper;
using UserWatchingService.Dtos.UserDto;
using UserWatchingService.Entities;

namespace UserWatchingService.MapperProfiles
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserReadDto>();
            CreateMap<User, UserConfirmationDto>();
        }
    }
}
