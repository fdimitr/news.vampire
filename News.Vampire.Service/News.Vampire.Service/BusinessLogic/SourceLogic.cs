using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;

namespace News.Vampire.Service.BusinessLogic
{
    public class SourceLogic : BaseLogic<Source>, ISourceLogic
    {
        public SourceLogic(DataContext dbContext) : base(dbContext)
        {
        }

        public async Task<IList<Source>> GetAll()
        {
            return await(from source in DbContext.Sources select source).ToListAsync();
        }

        public async Task<IList<Source>> GetSourcesReadyToLoadAsync()
        {
            var timestampNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return await (from source in DbContext.Sources
                join groupEntity in DbContext.Groups on source.GroupId equals groupEntity.Id
                where source.NextLoadedTime < timestampNow && groupEntity.IsActive
                select source).ToListAsync();
        }
    }
}
