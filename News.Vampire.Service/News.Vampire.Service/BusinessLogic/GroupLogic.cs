using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace News.Vampire.Service.BusinessLogic
{
    public class GroupLogic : BaseLogic<Group>, IGroupLogic
    {
        public GroupLogic(IDbContextFactory<DataContext> dbContextFactory) : base(dbContextFactory)
        {
        }

        public async Task<IList<Group>> GetAllAsync()
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            return await dbContext.Groups.ToListAsync();
        }
    }
}
