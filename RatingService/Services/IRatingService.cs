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

        List<RatingDTO> GetAllRatingsByUser(int userID);//sve ocene koje je dao korisnik

        List<RatingDTO> GetRatingByPostID(int postID, int userID);

        RatingDTO GetRatingByID(Guid ratingID);

        RatingDTO CreateRating(RatingCreationDTO rating, int userId);

        void  UpdateRating(RatingModifyingDTO rating, Guid ratingID);

        void DeleteRating(Guid ratingID);

        RatingDTO CheckDidIAlreadyRate(int userID, int postID);

        bool CheckDoIFollowUser(int userID, int followingID);

        bool CheckDidIBlockUser(int userID, int blockedID);

    }
}
