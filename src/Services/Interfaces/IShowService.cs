using System.Threading.Tasks;
using NetflixReviewService.Models;

namespace NetflixReviewService.Services.Interfaces
{
    public interface IShowService
    {
        Task<ShowModel> GetShow(string id);
        Task<NetflixData> GetShows();
    }
}
