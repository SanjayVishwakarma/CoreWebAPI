using HackerNewsWebApi.Business.Services;
using HackerNewsWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HackerNewsWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StoryManagementController : ControllerBase
    {
        private readonly IStoryService _storyService;
        
        public StoryManagementController(IStoryService storyService)
        {
            _storyService = storyService;
        }
        [HttpPost]
        public async Task<List<StoryDetailModel>> LoadStories(LoadStoriesRequestModel loadStoriesRequest)
        {
            List<StoryDetailModel> stories = new List<StoryDetailModel>();
            if (loadStoriesRequest.PageSize == 0 || !string.IsNullOrEmpty(loadStoriesRequest.SearchText))
            {
                loadStoriesRequest.PageSize = 200;
            }
            var response = await _storyService.BestStoriesAsync(loadStoriesRequest.PageNumber , loadStoriesRequest.PageSize);
                    if (response.IsSuccessStatusCode)
                    {
                        

                        var storiesResponse = response.Content.ReadAsStringAsync().Result;
                        var bestIds = JsonConvert.DeserializeObject<List<int>>(storiesResponse).Skip((loadStoriesRequest.PageNumber - 1) * loadStoriesRequest.PageSize)
                    .Take(loadStoriesRequest.PageSize).ToList();

                        var tasks = bestIds.Select(GetStoryAsync);
                        stories = (await Task.WhenAll(tasks)).ToList()
                    ;

                        if (!String.IsNullOrEmpty(loadStoriesRequest.SearchText))
                        {
                            var search = loadStoriesRequest.SearchText.ToLower();
                            stories = stories.Where(s =>
                                               s.Title.ToLower().IndexOf(search) > -1 )
                                               .ToList();
                        }
                    }
                    return stories;
        }

        private async Task<StoryDetailModel> GetStoryAsync(int storyId)
        {
            StoryDetailModel story = new StoryDetailModel();

            var response = await _storyService.GetStoryByIdAsync(storyId);
            if (response.IsSuccessStatusCode)
            {
                var storyResponse = response.Content.ReadAsStringAsync().Result;
                story = JsonConvert.DeserializeObject<StoryDetailModel>(storyResponse);
            }

            return story;
        }

        
    }
}
