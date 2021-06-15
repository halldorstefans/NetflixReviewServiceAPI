using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetflixReviewService.Models;
using NetflixReviewService.Services.Interfaces;

namespace NetflixReviewService.Controllers
{
    [ApiController]
    [Route("[controller]/{showId}")]
    public class ShowReviewsController : ControllerBase
    {
        private readonly IShowService _showService;
        private readonly ShowsContext _context;

        public ShowReviewsController(IShowService showService, ShowsContext context)
        {
            _showService = showService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostShowReview([FromRoute] string showId, ShowReviewDTO showReviewDTO)
        {
            var show = await _showService.GetShow(showId);

            if (show.Id == null)
            {
                return NotFound();
            }

            var showReview = new ShowReviewModel
            {
                ShowId = showId,
                Rating = showReviewDTO.Rating,
                Description = showReviewDTO.Description,
            };

            _context.ShowReviews.Add(showReview);
            await _context.SaveChangesAsync();

            var route = Request?.Path.Value;
            return Created(route + "/" + showReview.Id, ReviewToDTO(showReview));
        }

        private static ShowReviewDTOOut ReviewToDTO(ShowReviewModel showReview) =>
            new ShowReviewDTOOut
            {
                Id = showReview.Id,
                ShowId = showReview.ShowId,
                Rating = showReview.Rating,
                Description = showReview.Description,
            };
    }
}
