using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using System.Threading.Tasks;

namespace News.Vampire.Service.BusinessLogic
{
    public class BaseLogic<T> : IBaseLogic<T> where T : class
    {
        protected IDbContextFactory<DataContext> _dbContextFactory { get; }

        public BaseLogic(IDbContextFactory<DataContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> CreateAsync(T entity)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            var result = dbContext.Set<T>().Add(entity);
            await dbContext.SaveChangesAsync();
            return result.Entity;

        }

        public async Task UpdateAsync(T entity)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            dbContext.Set<T>().Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            T entity = await dbContext.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            dbContext.Set<T>().Remove(entity);
            var result = await dbContext.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> EntityExists(int id)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            return await dbContext.Set<T>().FindAsync(id) != null;
        }

    }
}
