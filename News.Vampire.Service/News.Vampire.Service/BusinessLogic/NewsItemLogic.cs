using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;

namespace News.Vampire.Service.BusinessLogic
{
    public class NewsItemLogic : BaseLogic<NewsItem>, INewsItemLogic
    {
        private readonly ISubscriptionLogic _subscriptionLogic;
        public NewsItemLogic(DataContext dbContext, ISubscriptionLogic subscriptionLogic) : base(dbContext)
        {
            _subscriptionLogic = subscriptionLogic;
        }

        public async Task<bool> ExistsAsync(long sourceId, string externalId)
        {
            return await DbContext.NewsItem.AnyAsync(n => n.SourceId == sourceId && n.ExternalId == externalId);
        }

        public async Task<List<NewsItem>> GetLatestNewsBySubscriptionAsync(int userId)
        {
            var result = new List<NewsItem>();
            var subscriptions = await _subscriptionLogic.GetSubscriptionsByUserAsync(userId);
            foreach (var subscription in subscriptions)
            {
                var lastLoaded = subscription.LastLoadedTime ?? 0;
                result.AddRange(await (from news in DbContext.NewsItem
                    where news.SourceId == subscription.SourceId && news.TimeStamp > lastLoaded
                    select news).ToListAsync());

                subscription.LastLoadedTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                await _subscriptionLogic.UpdateAsync(subscription);
            }

            return result;
        }
    }
}
