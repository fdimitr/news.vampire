using News.Vampire.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace News.Vampire.Service.BusinessLogic.Interfaces
{
    public interface ISubscriptionLogic : IBaseLogic<Subscription>
    {
        Task<List<Subscription>> GetSubscriptionsByUserAsync(int userId);
    }
}
