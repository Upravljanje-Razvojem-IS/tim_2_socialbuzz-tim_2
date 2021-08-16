using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockService.Mocks
{
    /// <summary>
    /// DTO za model pracenja korisnika
    /// </summary>
    public class FollowingMockDTO
    {
        /// <summary>
        /// ID pracenja
        /// </summary>
        private int followingID;

        public int FollowingID
        {
            get { return followingID; }
            set { followingID = value; }
        }


        /// <summary>
        /// ID korisnika koji je zapratio nekog drugog korisnika
        /// </summary>
        private int followerID;

        public int FollowerID
        {
            get { return followerID; }
            set { followerID = value; }
        }

        /// <summary>
        /// ID korisnika koji je zapracen od strane korisnika
        /// </summary>
        private int followedID;

        public int FollowedID
        {
            get { return followedID; }
            set { followedID = value; }
        }
    }
}
