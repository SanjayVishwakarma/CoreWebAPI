using HackerNewsWebApi.Business.Services;
using HackerNewsWebApi.Controllers;
using HackerNewsWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Newtonsoft.Json;
using System.Net;

namespace HackerNewsWebApi.Tests.TestCases
{
    public class StoryManagementControllerTest
    {
        private readonly StoryManagementController _controller;
        private readonly Mock<IStoryService> _mockService;
        private readonly Mock<IMemoryCache> memoryCache;

        public StoryManagementControllerTest()
        {
            memoryCache = new Mock<IMemoryCache>();
            _mockService = new Mock<IStoryService>();
            _controller = new StoryManagementController(_mockService.Object);
        }

        [Fact]
        public async Task LoadStories()
        {
            // Arrange
            var mockNews = new List<StoryDetailModel>
            {
                new StoryDetailModel {Title="Reverse engineering Ticketmaster's rotating barcodes",By="miki123211",Url="https://conduition.io/coding/ticketmaster/"},
                new StoryDetailModel {Title="AT&T says criminals stole phone records of 'nearly all' customers in data breach",By="impish9208",Url="https://techcrunch.com/2024/07/12/att-phone-records-stolen-data-breach/"}
            };
            var mockHttpClient = new Mock<HttpClient>();
            var response = await mockHttpClient.Object.GetAsync("https://hacker-news.firebaseio.com/v0/beststories.json");
            _mockService.Setup(s => s.BestStoriesAsync(1,1)).ReturnsAsync(response);
            // Mock GetStoryByIdAsync method for each story ID
            var story1 = await mockHttpClient.Object.GetAsync(string.Format("https://hacker-news.firebaseio.com/v0/item/{0}.json", 40944505));
            var story2 = await mockHttpClient.Object.GetAsync(string.Format("https://hacker-news.firebaseio.com/v0/item/{0}.json", 40950584));
            var story3 = await mockHttpClient.Object.GetAsync(string.Format("https://hacker-news.firebaseio.com/v0/item/{0}.json", 40915082));

            memoryCache.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                           .Returns(Mock.Of<ICacheEntry>());

            _mockService.Setup(s => s.GetStoryByIdAsync(40944505)).ReturnsAsync(story1);
            var request = new LoadStoriesRequestModel()
            {
                PageNumber = 1,
                PageSize = 1,
                SearchText = string.Empty
            };

            // Act
            var result = await _controller.LoadStories(request);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // Assuming only story1 and story2 match the search criteria
        }

    }
}
