using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NetflixReviewService.Helpers;
using NetflixReviewService.Models;
using NetflixReviewService.Services;
using NetflixReviewService.UnitTests.Mocks.Data;
using NetflixReviewService.UnitTests.Mocks.Services;
using NetflixReviewService.UnitTests.TestHelpers;
using Xunit;

namespace NetflixReviewService.UnitTests.Services
{
    public class ShowReviewsControllerTests
    {
        [Fact]
        public async void GetShows_ReturnAllShows()
        {
            // Arrange
            var mockSettings = new MockNetflixSettings();
            var settings = new NetflixSettings{
                AppBaseUrl = mockSettings.AppBaseUrl,
                TokenUrl = mockSettings.TokenUrl,
                ClientId = mockSettings.ClientId,
                ClientSecret = mockSettings.ClientSecret,
                Scope = mockSettings.Scope
            };
            var options = Options.Create(settings);
            var token = Task.Run(() => "R4nd0m7ok3n");
            var tokenService = new MockTokenService().MockGetToken(token);
            var getShowsResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new FakeHttpContent(JsonSerializer.Serialize(new NetflixData{
                    Data = new List<ShowModel>()
                    {
                        new ShowModel()
                        {
                            Id = "s2",
                            Title = "Titanic"
                        },
                        new ShowModel()
                        {
                            Id = "s1",
                            Title = "Star Wars"
                        }
                    }
                })),
                RequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get
                }
            };
            var client = HttpClientSetup(getShowsResponse);
            var showService = new NetflixShowService(tokenService.Object, options, client);


            // Act
            var result = await showService.GetShows();

            // Assert
            Assert.NotEmpty(result.Data);
            Assert.Equal("Star Wars", result.Data[1].Title);
            tokenService.VerifyGetToken(Times.Once());
        }

        [Fact]
        public async void GetShow_ReturnSingleShow()
        {
            // Arrange
            var mockSettings = new MockNetflixSettings();
            var settings = new NetflixSettings{
                AppBaseUrl = mockSettings.AppBaseUrl,
                TokenUrl = mockSettings.TokenUrl,
                ClientId = mockSettings.ClientId,
                ClientSecret = mockSettings.ClientSecret,
                Scope = mockSettings.Scope
            };
            var options = Options.Create(settings);
            var token = Task.Run(() => "R4nd0m7ok3n");
            var tokenService = new MockTokenService().MockGetToken(token);
            var getShowsResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new FakeHttpContent(JsonSerializer.Serialize(new ShowModel()
                        {
                            Id = "s1",
                            Title = "Star Wars"
                        })),
                RequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get
                }
            };
            var client = HttpClientSetup(getShowsResponse);
            var showService = new NetflixShowService(tokenService.Object, options, client);


            // Act
            var result = await showService.GetShow("s1");

            // Assert
            Assert.Equal("Star Wars", result.Title);
            tokenService.VerifyGetToken(Times.Once());
        }

        [Fact]
        public async void GetShow_ReturnShowNotFound()
        {
            // Arrange
            var mockSettings = new MockNetflixSettings();
            var settings = new NetflixSettings{
                AppBaseUrl = mockSettings.AppBaseUrl,
                TokenUrl = mockSettings.TokenUrl,
                ClientId = mockSettings.ClientId,
                ClientSecret = mockSettings.ClientSecret,
                Scope = mockSettings.Scope
            };
            var options = Options.Create(settings);
            var token = Task.Run(() => "R4nd0m7ok3n");
            var tokenService = new MockTokenService().MockGetToken(token);
            var getShowsResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new FakeHttpContent(JsonSerializer.Serialize(new ShowModel())),
                RequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get
                }
            };
            var client = HttpClientSetup(getShowsResponse);
            var showService = new NetflixShowService(tokenService.Object, options, client);


            // Act
            var result = await showService.GetShow("s100");

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Id);
            tokenService.VerifyGetToken(Times.Once());
        }

        private HttpClient HttpClientSetup(HttpResponseMessage responseMessage)
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", 
                    ItExpr.IsAny<HttpRequestMessage>(), 
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            return new HttpClient(mockHttpMessageHandler.Object);
        }
    }
}
