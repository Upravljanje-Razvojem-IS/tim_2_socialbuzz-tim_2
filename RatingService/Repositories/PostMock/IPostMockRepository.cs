using RatingService.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Repositories.PostMock
{
    public interface IPostMockRepository
    {
        PostMockDTO GetPostById(int postId);

        List<PostMockDTO> GetPostsByUserId(int userID);
    }
}
