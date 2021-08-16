using RatingService.DTO;
using System.Collections.Generic;

namespace RatingService.Services
{
    public interface IRatingTypeService
    {
        List<RatingTypeDTO> GetAllRatingTypes();

        RatingTypeDTO GetRatingTypeByID(int ratingTypeID);

        RatingTypeDTO CreateRatingType(RatingTypeCreationDTO ratingType);

        void UpdateRatingType(RatingTypeModifyingDTO ratingType, int typeID);

        void DeleteRatingType(int ratingTypeID);

    }
}
