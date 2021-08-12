using AutoMapper;
using RatingService.DTO;
using RatingService.Entities;
using RatingService.Exceptions;
using RatingService.Repositories;
using RatingService.Repositories.PostMock;
using RatingService.Repositories.UserMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Services
{
    public class RatingsService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IPostMockRepository _postMockRepository;
        private readonly IRatingTypeRepository _ratingTypeRepository;
        private readonly IUserMockRepository _userMockRepository;
        private readonly IMapper mapper;

        public RatingsService(IRatingRepository ratingRepository, IPostMockRepository postMockRepository,
                             IRatingTypeRepository ratingTypeRepository, IUserMockRepository userMockRepository, IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _postMockRepository = postMockRepository;
            _ratingTypeRepository = ratingTypeRepository;
            _userMockRepository = userMockRepository;
            this.mapper = mapper;
        }

        public RatingDTO CheckDidIAlreadyRate(int userID, int postID)
        {
            var rate = _ratingRepository.CheckDidIAlreadyRate(userID, postID);
            return mapper.Map<RatingDTO>(rate);
        }

        public bool CheckDidIBlockUser(int userID, int blockedID)
        {
            return _ratingRepository.CheckDidIBlockUser(userID, blockedID);
        }

        public bool CheckDoIFollowUser(int userID, int followingID)
        {
            return _ratingRepository.CheckDoIFollowUser(userID, followingID);
        }

        public RatingDTO CreateRating(RatingCreationDTO rating, int userId)//originalna metoda nema userid-vidi ovo-id onog ko ocenjuje
        {
            var user = _userMockRepository.GetUserByID(userId);

            if (user == null)
            {
                throw new NotFoundException("Not find user with that ID found...");
            }

            if (_postMockRepository.GetPostById(rating.PostID) == null)
            {
                throw new NotFoundException("Post with that ID does not exist!");
            }

            if (_ratingTypeRepository.GetRatingTypeByID(rating.RatingTypeID) == null)
            {
                throw new NotFoundException("Type of rating with that ID does not exist!");
            }

            Rating entity = mapper.Map<Rating>(rating);
            entity.UserID = userId;
            entity.RatingDate = DateTime.Now;

            var post = _postMockRepository.GetPostById(rating.PostID);
            var userThatPostedId = post.UserID;

            if (_ratingRepository.CheckDidIBlockUser(userId, userThatPostedId))
            {
                throw new BlockingException("You have blocked this user and you can not rate to his posts.");
            }

            if (!_ratingRepository.CheckDoIFollowUser(userId, userThatPostedId))
            {
                throw new BlockingException( "You are not following this user and you can not rate to his posts.");
            }

            if (_ratingRepository.CheckDidIAlreadyRate(userId, rating.PostID) != null)
            {
                throw new ErrorOccurException("You have already rate to this post.");
            }

            try
            {
                var rate = _ratingRepository.CreateRating(entity);
                _ratingRepository.SaveChanges();
                return mapper.Map<RatingDTO>(rate);
            }
            catch(Exception ex)
            {
                throw new ErrorOccurException(ex.Message);
            }
            

        }

        public void DeleteRating(Guid ratingID)
        {
            var rate = _ratingRepository.GetRatingByID(ratingID);

            if (rate == null)
            {
                throw new NotFoundException( "There is no rating with that ID!");
            }
            try
            {
                _ratingRepository.DeleteRating(ratingID);
                _ratingRepository.SaveChanges();
            }
           
            catch (Exception ex)
            {
                throw new ErrorOccurException( "Error deleting reaction: " + ex.Message);
            }
        }

        public List<RatingDTO> GetAllRatingsForUser(int userID)
        {
            var userId = _userMockRepository.GetUserByID(userID);

            if (userId == null)
            {
                throw new NotFoundException("Not find user with that ID found...");
            }

           
            
            var ratings = _ratingRepository.GetAllRatingsForUser(userID, _postMockRepository.GetPostsByUserId(userID).Select(p=>p.PostID).ToList());

            if (ratings == null || ratings.Count == 0)
            {
                throw new NotFoundException("There are no ratings for this user...");
            }

            return mapper.Map<List<RatingDTO>>(ratings);
        }

        public RatingDTO GetRatingByID(Guid ratingID)
        {
            var rateId = _ratingRepository.GetRatingByID(ratingID);

            if (rateId == null)
            {
                throw new NotFoundException("No rating with that ID found...");
            }

            var rating = _ratingRepository.GetRatingByID(ratingID);
            return mapper.Map<RatingDTO>(rating);
        }

        public List<RatingDTO> GetRatingByPostID(int postID, int userID)
        {
            var postId = _postMockRepository.GetPostById(postID);

            if (postId == null)
            {
                throw new NotFoundException("No post with that ID found...");
            }
            var userThatPostedId = _postMockRepository.GetPostById(postID).UserID;

            if (_ratingRepository.CheckDidIBlockUser(userID, userThatPostedId))
            {
                throw new BlockingException("You can not see this user's posts!"); //User 1 blokirao Usera 2
            }
            var ratings = _ratingRepository.GetRatingByPostID(postID, userID);

            if (ratings == null || ratings.Count == 0)
            {
                throw new ErrorOccurException( "This post doesn't have any ratings yet..."); //Post 2
            }

            return mapper.Map<List<RatingDTO>>(ratings);
        }

        public void UpdateRating(RatingModifyingDTO rating, Guid ratingID)
        {
            if (_ratingRepository.GetRatingByID(ratingID) == null)
            {
                throw new NotFoundException("Rating with that ID does not exist!");
            }

            if (_ratingTypeRepository.GetRatingTypeByID(rating.RatingTypeID) == null)
            {
                throw new NotFoundException("Type of rating with that ID does not exist!");
            }

            var oldRate = _ratingRepository.GetRatingByID(ratingID);
            var newRate = mapper.Map<Rating>(rating);

            if (oldRate.PostID != newRate.PostID)
            {
                throw new ErrorOccurException("Post ID can not be changed!");
            }

            try
            {
                newRate.UserID = oldRate.UserID;

                 mapper.Map(newRate, oldRate);
                _ratingRepository.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new ErrorOccurException("Error updating reaction: " + ex.Message); 

            }
        }

        public List<RatingDTO> GetAllRatings()
        {
            var ratings = _ratingRepository.GetAllRatings();
            return mapper.Map<List<RatingDTO>>(ratings);
        }

        public void CreateRating(RatingDTO rating)
        {
            throw new NotImplementedException();
        }

        public List<RatingDTO> GetAllRatingsByUser(int userID)
        {
            var userId = _userMockRepository.GetUserByID(userID);

            if (userId == null)
            {
                throw new NotFoundException("There is no user with that ID ...");
            }

            var ratings = _ratingRepository.GetAllRatingsByUser(userID);

            if (ratings == null || ratings.Count == 0)
            {
                throw new ErrorOccurException("This user has not yet given any rate...");
            }

            return mapper.Map<List<RatingDTO>>(ratings);
        }
    }
}
