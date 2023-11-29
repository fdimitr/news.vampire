using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;

namespace News.Vampire.Service.BusinessLogic
{
    public class UserGroupLogic : BaseLogic<UserGroup>, IUserGroupLogic
    {
        public UserGroupLogic(DataContext dbContext) : base(dbContext)
        {
        }

        public async Task<IList<UserGroup>> GetAllByUserAsync(long userId)
        {
            return await (from userGroup in DbContext.UserGroups
                where userGroup.UserId == userId
                select userGroup).Distinct().Include(ug => ug.Subscriptions)!.ThenInclude(s => s.Source).ToListAsync();
        }
    }
}
