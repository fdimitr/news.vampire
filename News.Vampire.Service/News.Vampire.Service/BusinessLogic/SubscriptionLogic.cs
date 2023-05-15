using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Vampire.Service.BusinessLogic
{
    public class SubscriptionLogic : BaseLogic<Subscription>, ISubscriptionLogic
    {
        public SubscriptionLogic(IDbContextFactory<DataContext> dbContextFactory) : base(dbContextFactory)
        {
        }

        public async Task<List<Subscription>> GetSubscriptionsByUserAsync(int userId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            return await (from subscr in dbContext.Subscriptions
                          where subscr.UserId == userId
                          select subscr).ToListAsync();
        }
    }
}
