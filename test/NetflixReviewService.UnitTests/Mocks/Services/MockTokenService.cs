using System.Threading.Tasks;
using Moq;
using NetflixReviewService.Services.Interfaces;

namespace NetflixReviewService.UnitTests.Mocks.Services
{
    public class MockTokenService : Mock<ITokenService>
    {
        public MockTokenService MockGetToken(Task<string> token)
        {
            Setup(x => x.GetToken()).Returns(token);

            return this;
        }

        public MockTokenService VerifyGetToken(Times times)
        {
            Verify(x => x.GetToken(), times);

            return this;
        }
    }
}