using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;

namespace News.Vampire.Service.BusinessLogic
{
    public class SubscriptionLogic : BaseLogic<Subscription>, ISubscriptionLogic
    {
        public SubscriptionLogic(DataContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Subscription>> GetSubscriptionsByUserAsync(int userId)
        {
            return await (from subscr in DbContext.Subscriptions
                where subscr.UserId == userId
                select subscr).ToListAsync();
        }
    }
}
