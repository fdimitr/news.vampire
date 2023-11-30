using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;

namespace News.Vampire.Service.BusinessLogic
{
    public class SourceLogic : BaseLogic<Source>, ISourceLogic
    {
        public SourceLogic(DataContext dbContext, DbContextOptions<DataContext> dbContextOptions) : base(dbContext, dbContextOptions)
        {
        }

        public IQueryable<Source> GetAll()
        {
            return from source in DbContext.Sources select source;
        }

        public async Task<IList<Source>> GetSourcesReadyToLoadAsync()
        {
            await using var dbContextTransactional = new DataContext(DbContextOptions);

            var timestampNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return await (from source in dbContextTransactional.Sources
                          join groupEntity in dbContextTransactional.Groups on source.GroupId equals groupEntity.Id
                          where source.NextLoadedTime < timestampNow && groupEntity.IsActive
                          select source).ToListAsync();
        }
    }
}
