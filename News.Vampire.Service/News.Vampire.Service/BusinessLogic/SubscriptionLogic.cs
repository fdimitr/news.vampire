using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;

namespace News.Vampire.Service.BusinessLogic
{
    public class SubscriptionLogic : BaseLogic<Subscription>, ISubscriptionLogic
    {
        public SubscriptionLogic(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public async Task<List<Subscription>> GetSubscriptionsByUserAsync(int userId)
        {
            using (var dbContext = new DataContext(_dbContextOptions))
            {
                return await (from subscr in dbContext.Subscriptions
                              where subscr.UserId == userId
                              select subscr).ToListAsync();
            }
        }
    }
}
