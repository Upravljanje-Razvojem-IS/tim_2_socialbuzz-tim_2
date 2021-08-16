using RatingService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Repositories
{
    public class RatingTypeRepository : IRatingTypeRepository
    {
        private readonly ContextDB contextDB;

        public RatingTypeRepository (ContextDB contextDB)
        {
            this.contextDB = contextDB;
        }
        public RatingType CreateRatingType(RatingType ratingType)
        {
            var type = contextDB.Add(ratingType);
            return type.Entity;
        }

        public void DeleteRatingType(int ratingTypeID)
        {
            var type = GetRatingTypeByID(ratingTypeID);
            contextDB.Remove(type);
        }

        public List<RatingType> GetAllRatingTypes()
        {
            return contextDB.RatingType.ToList();
        }

        public RatingType GetRatingTypeByID(int ratingTypeID)
        {
            return contextDB.RatingType.FirstOrDefault(e => e.RatingTypeID == ratingTypeID);
        }

        public bool SaveChanges()
        {
            return contextDB.SaveChanges() > 0;
        }

        public void UpdateRatingType(RatingType ratingType)
        {
            throw new NotImplementedException(); // kasnije se implementira
        }
    }
}
