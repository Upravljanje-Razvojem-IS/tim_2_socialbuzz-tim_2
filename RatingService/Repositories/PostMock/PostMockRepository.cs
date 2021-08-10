using RatingService.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Repositories.PostMock
{
    public class PostMockRepository : IPostMockRepository
    {
        public static List<PostMockDTO> Posts { get; set; } = new List<PostMockDTO>();

        public PostMockRepository()
        {
            FillData();
        }

        private void FillData()
        {
            Posts.AddRange(new List<PostMockDTO>
            {
                new PostMockDTO
                {
                    PostID = 1,
                    PostTitle = "Samsung Galaxy S21 5G",
                    PostDescription = "Povoljno prodajem Samsung Galaxy S21!",
                    UserID = 1,
                    ConditionID = 1,
                    PostTypeID = 1,
                    PostedOn = DateTime.Parse("2021-04-21T09:00:00")
                },
                new PostMockDTO
                {
                   PostID = 2,
                   PostTitle = "Gwendy's Final Task by Stephen King",
                   PostDescription = "Prodajem novu knjigu Stivena Kinga!",
                   UserID = 2,
                   ConditionID = 2,
                   PostTypeID = 2,
                   PostedOn = DateTime.Parse("2021-04-21T09:00:00")
                },
                 new PostMockDTO
                {
                    PostID = 3,
                    PostTitle = "Vintage crna kozna torbica",
                    PostDescription = "Od prirodne koze i u dobrom je stanju, bez ostecenja",
                    UserID = 3,
                    ConditionID = 3,
                    PostTypeID = 3,
                    PostedOn = DateTime.Parse("2021-04-21T09:00:00")
                },
                   new PostMockDTO
                {
                    PostID = 4,
                    PostTitle = "Gradski bicikl Carma plavi 28",
                    PostDescription = "Prelep gradski bicikl. Udobno sedište, mekane ručkice Herrmans, kvalitetna ratan korpa",
                    UserID = 4,
                    ConditionID = 4,
                    PostTypeID = 4,
                    PostedOn = DateTime.Parse("2021-08-10T09:45:00")
                }
            });
        }

        public PostMockDTO GetPostById(int postId)
        {
            return Posts.FirstOrDefault(e => e.PostID == postId);
        }

        public List<PostMockDTO> GetPostsByUserId(int userID)
        {
            var res = Posts.Where(post => post.UserID == userID).ToList();
            return res;
        }
    }
}
