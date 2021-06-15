namespace NetflixReviewService.UnitTests.Mocks.Data
{
    public class MockNetflixSettings
    {
        public string AppBaseUrl { get; set; } = "https://netflixService.test/v1/";
        public string TokenUrl { get; set; } = "https://netflix-auth.test/connect/token";
        public string ClientId { get; set; } = "Tester";
        public string ClientSecret { get; set; } = "TopSecret";
        public string Scope { get; set; } = "netflix.shows.test";
    }
}