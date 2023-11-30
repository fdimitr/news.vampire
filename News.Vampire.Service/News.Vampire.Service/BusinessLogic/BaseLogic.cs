using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;

namespace News.Vampire.Service.BusinessLogic
{
    public class BaseLogic<T> : IBaseLogic<T> where T : class
    {
        protected DataContext DbContext { get; set; }
        protected DbContextOptions<DataContext> DbContextOptions { get; set; }

        public BaseLogic(DataContext dbContext, DbContextOptions<DataContext> dbContextOptions)
        {
            DbContext = dbContext;
            DbContextOptions = dbContextOptions;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            await using var dbContextTransactional = new DataContext(DbContextOptions);
            return await dbContextTransactional.Set<T>().FindAsync(id);
        }

        public async Task<T> CreateAsync(T entity)
        {
            await using var dbContextTransactional = new DataContext(DbContextOptions);
            var result = dbContextTransactional.Set<T>().Add(entity);
            await dbContextTransactional.SaveChangesAsync();
            return result.Entity;
        }

        public async Task UpdateAsync(T entity)
        {
            await using var dbContextTransactional = new DataContext(DbContextOptions);
            dbContextTransactional.Set<T>().Attach(entity);
            dbContextTransactional.Entry(entity).State = EntityState.Modified;
            await dbContextTransactional.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var dbContextTransactional = new DataContext(DbContextOptions);
            T? entity = await dbContextTransactional.Set<T>().FindAsync(id);
            if (entity is null)
            {
                return false;
            }

            dbContextTransactional.Set<T>().Remove(entity);
            var result = await dbContextTransactional.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> EntityExists(int id)
        {
            await using var dbContextTransactional = new DataContext(DbContextOptions);
            return await dbContextTransactional.Set<T>().FindAsync(id) != null;
        }

    }
}
