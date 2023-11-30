using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;

namespace News.Vampire.Service.BusinessLogic
{
    public class GroupLogic : BaseLogic<Group>, IGroupLogic
    {

        public GroupLogic(DataContext dbContext, DbContextOptions<DataContext> dbContextOptions) : base(dbContext, dbContextOptions)
        {
        }

        public IQueryable<Group> GetAll()
        {
            return DbContext.Groups;
        }
    }
}
