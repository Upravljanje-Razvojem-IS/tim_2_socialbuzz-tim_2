using RatingService.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Repositories.FollowingMock
{
    public class FollowingMockRepository : IFollowingMockRepository
    {
        public static List<FollowingMockDTO> FollowingUsers { get; set; } = new List<FollowingMockDTO>();

        public FollowingMockRepository()
        {
            FillData();
        }

        private void FillData()
        {

            FollowingMockDTO mock = new FollowingMockDTO();
            mock.FollowingID = 1;
            mock.FollowerID = 1;
            mock.FollowedID = 3;

            FollowingMockDTO mock2 = new FollowingMockDTO();
            mock2.FollowingID = 2;
            mock2.FollowerID = 3;
            mock2.FollowedID = 2;

            FollowingMockDTO mock3 = new FollowingMockDTO();
            mock3.FollowingID = 3;
            mock3.FollowerID = 2;
            mock3.FollowedID = 3;

            FollowingMockDTO mock4 = new FollowingMockDTO();
            mock4.FollowingID = 4;
            mock4.FollowerID = 4;
            mock4.FollowedID = 1;

            FollowingUsers.Add(mock);
            FollowingUsers.Add(mock2);
            FollowingUsers.Add(mock3);
            FollowingUsers.Add(mock4);
        }

        public List<int> GetFollowedUsers(int userId)//lista svih koje korisnik prati
        {
            List<int> listOfFollowedUsers = new List<int>();

            var query = from l1 in FollowingUsers
                        select l1;

            foreach (var v in query)
            {
                if (v.FollowerID == userId)
                {
                    listOfFollowedUsers.Add(v.FollowedID);
                }

            }

            return listOfFollowedUsers;
        }

        public bool CheckDoIFollowUser(int userId, int followingId)
        {

            var query = from l1 in FollowingUsers
                        select l1;

            foreach (var v in query)
            {
                if (v.FollowerID == userId && v.FollowedID == followingId)
                {
                    return true;
                }

            }

            return false;
        }
    }
}
