using News.Vampire.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace News.Vampire.Service.BusinessLogic.Interfaces
{
    public interface INewsItemLogic: IBaseLogic<NewsItem>
    {
        Task<bool> ExistsAsync(long sourceId, string externalId);
        Task<List<NewsItem>> GetLatestNewsBySubscriptionAsync(int userId);
    }
}
