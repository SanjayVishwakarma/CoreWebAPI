using HackerNewsWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNewsWebApi.Business.Services
{
    public interface IStoryService
    {
        // Task<ActionResult<IEnumerable<StoryDetailModel>>> GetStories(int page = 1, int pageSize = 10);
        // Task<ActionResult<StoryDetailModel>> GetStory(int id);

        Task<HttpResponseMessage> BestStoriesAsync(int PageNumber, int PageSize);
        Task<HttpResponseMessage> GetStoryByIdAsync(int id);
    }
}
