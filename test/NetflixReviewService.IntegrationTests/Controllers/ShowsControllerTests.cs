using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NetflixReviewService.Models;
using Xunit;

namespace NetflixReviewService.IntegrationTests
{
    public class ShowsControllerTests : IntegrationTest
    {
        public ShowsControllerTests(ApiWebApplicationFactory fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetShows_ShouldReturnAllShows()
        {
            // Act
            var response = await _client.GetAsync("/shows");
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var data = JsonSerializer.Deserialize<NetflixData>(await response.Content.ReadAsStringAsync());

            Assert.NotEmpty(data.Data);
            Assert.Equal("s1", data.Data[0].Id);
        }

        [Fact]
        public async Task GetShows_ShouldReturnOneShow()
        {
            // Arrange
            var showId = "s2";

            // Act
            var response = await _client.GetAsync($"/shows/{showId}");
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var data = JsonSerializer.Deserialize<ShowModel>(await response.Content.ReadAsStringAsync());
            Assert.Equal(showId, data.Id);
        }

        [Fact]
        public async Task GetShows_ShouldReturnNotFoundShow()
        {
            // Arrange
            var showId = "r360";

            // Act
            var response = await _client.GetAsync($"/shows/{showId}");
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var data = JsonSerializer.Deserialize<ShowModel>(await response.Content.ReadAsStringAsync());
            Assert.Null(data.Id);
        }
    }
}