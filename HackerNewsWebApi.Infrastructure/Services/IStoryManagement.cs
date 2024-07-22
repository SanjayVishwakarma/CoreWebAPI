namespace CoreWebAPI.Infrastructure.Services
{
    public interface IStoryManagement
    {
        Task<HttpResponseMessage> BestStoriesAsync();
        Task<HttpResponseMessage> GetStoryByIdAsync(int id);
    }
}
