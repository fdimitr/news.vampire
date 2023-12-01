using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;

namespace News.Vampire.Service.BusinessLogic
{
    public class UserGroupLogic : BaseLogic<UserGroup>, IUserGroupLogic
    {
        public UserGroupLogic(DataContext dbContext, DbContextOptions<DataContext> dbContextOptions) : base(dbContext, dbContextOptions)
        {
        }

        public IQueryable<UserGroup> GetAllByUserAsync(int userId)
        {
            return (from userGroup in DbContext.UserGroups where userGroup.UserId == userId select userGroup).Distinct()
                .Include(ug => ug.Subscriptions)!.ThenInclude(s => s.Source);
        }
    }
}
