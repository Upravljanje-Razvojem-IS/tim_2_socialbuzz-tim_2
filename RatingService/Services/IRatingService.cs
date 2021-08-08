using RatingService.DTO;
using RatingService.Entities;
using System;
using System.Collections.Generic;

namespace RatingService.Services
{
    public interface IRatingService
    {
        List<RatingDTO> GetAllRatings();

        List<RatingDTO> GetAllRatingsForUser(int userID);//sve ocene koje je korisnik dobio

        List<Rating> GetAllRatingsByUser(int userID);//sve ocene koje je dao korisnik

        List<RatingDTO> GetRatingByPostID(int postID, int userID);

        RatingDTO GetRatingByID(Guid ratingID);

        void CreateRating(RatingDTO rating);

        void UpdateRating(RatingDTO rating);

        void DeleteRating(Guid ratingID);

        RatingDTO CheckDidIAlreadyRate(int userID, int postID);

        bool CheckDoIFollowUser(int userID, int followingID);

        bool CheckDidIBlockUser(int userID, int blockedID);

    }
}
