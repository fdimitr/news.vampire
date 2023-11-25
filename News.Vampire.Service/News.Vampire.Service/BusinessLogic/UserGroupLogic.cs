using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;

namespace News.Vampire.Service.BusinessLogic
{
    public class UserGroupLogic : BaseLogic<UserGroup>, IUserGroupLogic
    {
        public UserGroupLogic(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public async Task<IList<UserGroup>> GetAllByUserAsync(long userId)
        {
            await using var dbContext = new DataContext(_dbContextOptions);
            return await (from userGroup in dbContext.UserGroups
                where userGroup.UserId == userId
                select userGroup).Distinct().Include(ug => ug.Subscriptions)!.ThenInclude(s => s.Source).ToListAsync();
        }
    }
}
