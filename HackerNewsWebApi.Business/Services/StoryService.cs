using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text;

namespace HackerNewsWebApi.Business.Services
{
    public class StoryService : IStoryService
    {
        private static HttpClient client = new HttpClient();
        private IMemoryCache _cache;
        public StoryService(IMemoryCache cache)
        {
            this._cache = cache;
        }

        public async Task<HttpResponseMessage> BestStoriesAsync(int PageNumber, int PageSize)
        {
            // Calculate a unique cache key based on PageNumber and PageSize
            string cacheKey = $"{PageNumber}_{PageSize}";

            // Attempt to get the cached response for the given cache key
            var cachedResponse = await _cache.GetOrCreateAsync(cacheKey, async cacheEntry =>
            {
                // Set cache expiration policy
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10); // Cache for 10 minutes

                // Fetch the best stories from Hacker News API
                var response = await client.GetAsync("https://hacker-news.firebaseio.com/v0/beststories.json");

                // Return the response to be cached
                return await response.Content.ReadAsStringAsync();
            });

            // If the cached response is not null or empty, return it
            if (!string.IsNullOrEmpty(cachedResponse))
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(cachedResponse, Encoding.UTF8, "application/json")
                };
            }
            else
            {
                // If no cached response was found, return NotFound (or appropriate status code)
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }


        public async Task<HttpResponseMessage> GetStoryByIdAsync(int id)
        {

            var cachedResponse = await _cache.GetOrCreateAsync(id,
               async cacheEntry =>
               {
                   cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10); // Cache for 10 minutes

                   // Fetch the best stories from Hacker News API
                   var response = await client.GetAsync(string.Format("https://hacker-news.firebaseio.com/v0/item/{0}.json", id));

                   // Return the response to be cached
                   return await response.Content.ReadAsStringAsync();
               }
                );
            // If the cached response is not null or empty, return it
            if (!string.IsNullOrEmpty(cachedResponse))
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(cachedResponse, Encoding.UTF8, "application/json")
                };
            }
            else
            {
                // If no cached response was found, return NotFound (or appropriate status code)
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
    }
}
