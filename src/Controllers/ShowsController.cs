using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetflixReviewService.Models;
using NetflixReviewService.Services.Interfaces;

namespace NetflixReviewService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;
        private readonly ShowsContext _context;

        public ShowsController(IShowService showService, ShowsContext context)
        {
            _showService = showService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetShows()
        {            
            var shows = await _showService.GetShows();

            foreach (var show in shows.Data)
            {
                show.Reviews = _context.ShowReviews.Where(sr => sr.ShowId == show.Id).ToList();
            }

            return Ok(shows);
        }

        [HttpGet("{showId}")]
        public async Task<IActionResult> GetShow(string showId)
        {            
            var show = await _showService.GetShow(showId);

            if (show.Id == null)
            {
                return NotFound();
            }

            show.Reviews = _context.ShowReviews.Where(sr => sr.ShowId == showId).ToList();

            return Ok(show);
        }        
    }
}
