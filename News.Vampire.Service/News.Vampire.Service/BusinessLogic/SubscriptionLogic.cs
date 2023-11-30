using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;

namespace News.Vampire.Service.BusinessLogic
{
    public class SubscriptionLogic : BaseLogic<Subscription>, ISubscriptionLogic
    {
        public SubscriptionLogic(DataContext dbContext, DbContextOptions<DataContext> dbContextOptions) : base(dbContext, dbContextOptions)
        {
        }

        public async Task<List<Subscription>> GetSubscriptionsByUserAsync(int userId)
        {
            await using var dbContextTransactional = new DataContext(DbContextOptions);

            return await (from subscr in dbContextTransactional.Subscriptions
                where subscr.UserId == userId
                select subscr).ToListAsync();
        }
    }
}
