using Microsoft.EntityFrameworkCore;

namespace NetflixReviewService.Models
{
    public class ShowsContext : DbContext
    {
        public ShowsContext(DbContextOptions<ShowsContext> options)
            : base(options)
        {
        }

        public DbSet<ShowReviewModel> ShowReviews { get; set; }
    }
}