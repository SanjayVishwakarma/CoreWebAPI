using CoreWebAPI.Business.Services;

namespace CoreWebAPI.Business.Repositories
{
    public class StoryManagement : IStoryManagement
    {
        private static HttpClient client = new HttpClient();

        public StoryManagement()
        {

        }

        public async Task<HttpResponseMessage> BestStoriesAsync()
        {
            return await client.GetAsync("https://hacker-news.firebaseio.com/v0/beststories.json");
        }

        public async Task<HttpResponseMessage> GetStoryByIdAsync(int id)
        {
            return await client.GetAsync(string.Format("https://hacker-news.firebaseio.com/v0/item/{0}.json", id));
        }
    }
}
