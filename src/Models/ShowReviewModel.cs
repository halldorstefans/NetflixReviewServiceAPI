namespace NetflixReviewService.Models
{
    public class ShowReviewModel
    {
        public long Id { get; set; }
        public string ShowId { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
    }
}