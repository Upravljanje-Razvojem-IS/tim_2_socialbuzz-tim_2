using UserWatchingService.Entities;

namespace UserWatchingService.Interfaces
{
    public interface IAuthService
    {
        string Login(Principal principal);
        string GenerateJwt(string role);
    }
}
