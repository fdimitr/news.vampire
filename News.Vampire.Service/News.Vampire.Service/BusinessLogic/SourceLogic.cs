using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;

namespace News.Web.Api.BusinessLogic
{
    public class SourceLogic : BaseLogic<Source>, ISourceLogic
    {
        public SourceLogic(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public async Task<IList<Source>> GetAll()
        {
            using (var dbContext = new DataContext(_dbContextOptions))
            {
                return await(from source in dbContext.Sources
                             select source).ToListAsync();
            }
        }

        public async Task<IList<Source>> GetSourcesReadyToLoadAsync()
        {
            using (var dbContext = new DataContext(_dbContextOptions))
            {
                var timestampNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                return await (from source in dbContext.Sources
                              join groupEntity in dbContext.Groups on source.GroupId equals groupEntity.Id
                              where source.NextLoadedTime < timestampNow && groupEntity.isActive
                              select source).ToListAsync();
            }
        }
    }
}
