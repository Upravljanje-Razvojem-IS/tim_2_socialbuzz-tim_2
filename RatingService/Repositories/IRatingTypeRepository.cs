using RatingService.Entities;
using System;
using System.Collections.Generic;

namespace RatingService.Repositories
{
    public interface IRatingTypeRepository
    {
        List<RatingType> GetAllRatingTypes();

        RatingType GetRatingTypeByID(int ratingTypeID);

        RatingType CreateRatingType(RatingType ratingType);

        void UpdateRatingType(RatingType ratingType);

        void DeleteRatingType(int ratingTypeID);

        public bool SaveChanges();
    }
}
