using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Repositories.BlockingMock
{
    public interface IBlockingMockRepository
    {
        List<int> GetBlockedUsers(int userId);

        /// <summary>
        /// Provera da li samm blokirala korisnika
        /// </summary>
        /// <param name="userId">Moj Id</param>
        /// <param name="blockedId">Id korisnika za kog proveravam da li sam ga blokirala</param>
        /// <returns></returns>
        public bool CheckDidIBlockUser(int userId, int blockedId);
    }
}
