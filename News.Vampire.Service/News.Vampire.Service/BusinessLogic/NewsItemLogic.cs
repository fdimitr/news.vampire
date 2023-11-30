using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;

namespace News.Vampire.Service.BusinessLogic
{
    public class NewsItemLogic : BaseLogic<NewsItem>, INewsItemLogic
    {
        private readonly ISubscriptionLogic _subscriptionLogic;
        public NewsItemLogic(DataContext dbContext, DbContextOptions<DataContext> dbContextOptions, ISubscriptionLogic subscriptionLogic) : base(dbContext, dbContextOptions)
        {
            _subscriptionLogic = subscriptionLogic;
        }

        public async Task<bool> ExistsAsync(long sourceId, string externalId)
        {
            await using var dbContextTransactional = new DataContext(DbContextOptions);
            return await dbContextTransactional.NewsItem.AnyAsync(n => n.SourceId == sourceId && n.ExternalId == externalId);
        }

        public async Task<List<NewsItem>> GetLatestNewsBySubscriptionAsync(int userId)
        {
            await using var dbContextTransactional = new DataContext(DbContextOptions);

            var result = new List<NewsItem>();
            var subscriptions = await _subscriptionLogic.GetSubscriptionsByUserAsync(userId);
            foreach (var subscription in subscriptions)
            {
                var lastLoaded = subscription.LastLoadedTime ?? 0;
                result.AddRange(await (from news in dbContextTransactional.NewsItem
                    where news.SourceId == subscription.SourceId && news.TimeStamp > lastLoaded
                    select news).ToListAsync());

                subscription.LastLoadedTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                await _subscriptionLogic.UpdateAsync(subscription);
            }

            return result;
        }
    }
}
