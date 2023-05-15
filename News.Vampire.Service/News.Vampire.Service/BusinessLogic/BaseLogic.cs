using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using System.Threading.Tasks;

namespace News.Vampire.Service.BusinessLogic
{
    public class BaseLogic<T> : IBaseLogic<T> where T : class
    {
        protected DbContextOptions<DataContext> _dbContextOptions { get; set; }

        public BaseLogic(DbContextOptions<DataContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            using (var dbContext = new DataContext(_dbContextOptions))
            {
                return await dbContext.Set<T>().FindAsync(id);
            }
        }

        public async Task<T> CreateAsync(T entity)
        {
            using (var dbContext = new DataContext(_dbContextOptions))
            {
                var result = dbContext.Set<T>().Add(entity);
                await dbContext.SaveChangesAsync();
                return result.Entity;
            }
        }

        public async Task UpdateAsync(T entity)
        {
            using (var dbContext = new DataContext(_dbContextOptions))
            {
                dbContext.Set<T>().Attach(entity);
                dbContext.Entry(entity).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var dbContext = new DataContext(_dbContextOptions))
            {
                T entity = await dbContext.Set<T>().FindAsync(id);
                if (entity == null)
                {
                    return false;
                }

                dbContext.Set<T>().Remove(entity);
                var result = await dbContext.SaveChangesAsync();
                return result > 0 ? true : false;
            }
        }

        public async Task<bool> EntityExists(int id)
        {
            using (var dbContext = new DataContext(_dbContextOptions))
            {
                return await dbContext.Set<T>().FindAsync(id) != null;
            }
        }

    }
}
