using System.Threading.Tasks;

namespace NetflixReviewService.Services.Interfaces
{
    public interface ITokenService
    {
         Task<string> GetToken();
    }
}
