using CoreWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWebAPI.Business.Services
{
    public interface IStoryManagement
    {
       // Task<ActionResult<IEnumerable<StoryDetailModel>>> GetStories(int page = 1, int pageSize = 10);
       // Task<ActionResult<StoryDetailModel>> GetStory(int id);

        Task<HttpResponseMessage> BestStoriesAsync();
        Task<HttpResponseMessage> GetStoryByIdAsync(int id);
    }
}
