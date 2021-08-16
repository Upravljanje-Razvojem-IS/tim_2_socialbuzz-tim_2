using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Mocks
{
    /// <summary>
    /// DTO  model za pracenje korisnika
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
        /// ID korisnika koji je zapracen
        /// </summary>
        private int followedID;

        public int FollowedID
        {
            get { return followedID; }
            set { followedID = value; }
        }
    }
}
