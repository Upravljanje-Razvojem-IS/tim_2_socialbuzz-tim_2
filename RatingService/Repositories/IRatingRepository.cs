using RatingService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Repositories
{
    public interface IRatingRepository
    {
        List<Rating> GetAllRatings();//samo vraca sve ocene
        List<Rating> GetAllRatingsForUser(int userID, List<int> postsIDs);//sve ocene koje je korisnik dobio
        List<Rating> GetAllRatingsByUser(int userID);//sve ocene koje je neki korisnik dao

        List<Rating> GetRatingByPostID(int postID, int userID);

        public Rating GetRatingByID(Guid ratingID);

        Rating CreateRating(Rating rating);

        public void UpdateRating(Rating rating);

        public void DeleteRating(Guid ratingID);

        public Rating CheckDidIAlreadyRate(int userID, int postID);

        bool CheckDoIFollowUser(int userID, int followingID);

        bool CheckDidIBlockUser(int userID, int blockedID);

        bool SaveChanges();
    }
}
