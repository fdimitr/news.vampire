using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;

namespace News.Vampire.Service.BusinessLogic
{
    public class GroupLogic : BaseLogic<Group>, IGroupLogic
    {
        public GroupLogic(DbContextOptions<DataContext> dbContextOptions): base(dbContextOptions)
        {
        }

        public async Task<IList<Group>> GetAllAsync()
        {
            using (var dbContext = new DataContext(_dbContextOptions))
            {
                return await dbContext.Groups.ToListAsync();
            }
        }
    }
}
