using System.Threading.Tasks;
using Moq;
using NetflixReviewService.Models;
using NetflixReviewService.Services.Interfaces;

namespace NetflixReviewService.UnitTests.Mocks.Services
{
    public class MockShowService : Mock<IShowService>
    {
        public MockShowService MockGetShow(Task<ShowModel> show)
        {
            Setup(x => x.GetShow(It.IsAny<string>())).Returns(show);

            return this;
        }

        public MockShowService VerifyGetShow(Times times)
        {
            Verify(x => x.GetShow(It.IsAny<string>()), times);

            return this;
        }
    }
}