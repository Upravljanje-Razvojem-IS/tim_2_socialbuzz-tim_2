using Microsoft.EntityFrameworkCore;
using RatingService.Entities;
using RatingService.Repositories.BlockingMock;
using RatingService.Repositories.FollowingMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ContextDB contextDB;
        private readonly IBlockingMockRepository blockingMockRepository;
        private readonly IFollowingMockRepository followingMockRepository;

        public RatingRepository(ContextDB contextDB, IFollowingMockRepository followingMockRepository, IBlockingMockRepository blockingMockRepository)
        {
            this.contextDB = contextDB;
            this.followingMockRepository = followingMockRepository;
            this.blockingMockRepository = blockingMockRepository;
        }

        public Rating CheckDidIAlreadyRate(int userID, int postID)
        {
            return contextDB.Rating.FirstOrDefault(e => e.UserID == userID && e.PostID == postID);
        }

        public bool CheckDidIBlockUser(int userID, int blockedID)
        {
            return blockingMockRepository.CheckDidIBlockUser(userID, blockedID);
        }

        public bool CheckDoIFollowUser(int userID, int followingID)
        {
            return followingMockRepository.CheckDoIFollowUser(userID, followingID);
        }

        public Rating CreateRating(Rating rating)
        {
            var rate = contextDB.Add(rating);
            return rate.Entity;
        }

        public void DeleteRating(Guid ratingID)
        {
            var reaction = GetRatingByID(ratingID);
            contextDB.Remove(reaction);
        }

        public List<Rating> GetAllRatings()
        {
            return contextDB.Rating.ToList();
        }

        public List<Rating> GetAllRatingsByUser(int userID)
        {
            var query = from rate in contextDB.Rating
                        where rate.UserID == userID 
                        select rate;

            return query.ToList();
        }

        public List<Rating> GetAllRatingsForUser(int userID, List<int> postsIDs)//mora userid jer se proveravaju da li za tog usera koga se gledaju ocene ima ko je blokiran
        {
            var query = from rate in contextDB.Rating
                        where /*rate.UserID == userID && */ postsIDs.Contains(rate.PostID) &&
                        !(from o in blockingMockRepository.GetBlockedUsers(userID)
                          select o).Contains(rate.UserID)
                        select rate;

            return query.ToList();
        }

        public Rating GetRatingByID(Guid ratingID)
        {
            return contextDB.Rating.FirstOrDefault(e => e.RatingID == ratingID);
        }

        public List<Rating> GetRatingByPostID(int postID, int userID) //userid samo proverava da se prikazu ocene za post, koji su dali ljudi koje korinsik prati
        {
            var query = from rate in contextDB.Rating
                        where !(from o in blockingMockRepository.GetBlockedUsers(userID)
                               select o).Contains(rate.UserID)
                        where rate.PostID == postID
                        select rate;

            return query.ToList();
        }


        public bool SaveChanges()
        {
            return contextDB.SaveChanges() > 0;
        }

        public void UpdateRating(Rating rating)
        {
        }
    }
}
