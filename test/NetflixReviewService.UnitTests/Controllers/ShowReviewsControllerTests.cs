using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NetflixReviewService.Controllers;
using NetflixReviewService.Models;
using NetflixReviewService.UnitTests.Mocks.Services;
using Xunit;

namespace NetflixReviewService.UnitTests.Controllers
{
    public class ShowReviewsControllerTests
    {
        [Fact]
        public async void PostShowReview_ReturnShowReview()
        {
            // Arrange
            var show = Task.Run(() => new ShowModel()
                        {
                            Id = "s1",
                            Title = "Star Wars"
                        });
            var showService = new MockShowService().MockGetShow(show);
            var dbContext = await GetDatabaseContext();
            var reviewsController = new ShowReviewsController(showService.Object, dbContext);
            var showId = "s3";
            var myReview = new ShowReviewDTO{
                Rating = 5,
                Description = "Best movie ever"
            };

            // Act
            var result = await reviewsController.PostShowReview(showId, myReview);

            // Assert
            var okResult = result.Should().BeOfType<CreatedResult>().Subject;
            var reviewDTO = okResult.Value.Should().BeAssignableTo<ShowReviewDTOOut>().Subject;
            Assert.Equal(myReview.Rating, reviewDTO.Rating);
            showService.VerifyGetShow(Times.Once());
        }

        [Fact]
        public async void PostShowReview_ReturnInvalidReview()
        {
            // Arrange
            var show = Task.Run(() => new ShowModel()
                        {
                            Id = "s1",
                            Title = "Star Wars"
                        });
            var showService = new MockShowService().MockGetShow(show);
            var dbContext = await GetDatabaseContext();
            var reviewsController = new ShowReviewsController(showService.Object, dbContext);
            var myReview = new ShowReviewDTO{
                Rating = 100,
                Description = "Best movie ever"
            };

            // Act
            var validationResult = Validator.TryValidateObject(myReview, new ValidationContext(myReview, null, null), null, true);

            // Assert            
            Assert.False(validationResult);
        }


        private async Task<ShowsContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ShowsContext>()
                .UseInMemoryDatabase(databaseName: "ShowReviewsTest")
                .Options;
            var showsContext = new ShowsContext(options);
            showsContext.Database.EnsureCreated();
            if (await showsContext.ShowReviews.CountAsync() <= 0)
            {
                showsContext.ShowReviews.Add(new ShowReviewModel()
                {
                    Id = 1,
                    ShowId = "s1",
                    Rating = 3,
                    Description = "Very Good"                    
                });
                await showsContext.SaveChangesAsync();
            }
            return showsContext;
        }
    }
}