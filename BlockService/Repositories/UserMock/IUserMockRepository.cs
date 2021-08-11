
using BlockService.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockService.Repositories.UserMock
{
    public interface IUserMockRepository
    {
        List<UserMockDTO> GetAllUsers();

        UserMockDTO GetUserByID(int userID);
    }
}
