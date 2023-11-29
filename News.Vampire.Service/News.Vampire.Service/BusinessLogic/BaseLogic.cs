using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;

namespace News.Vampire.Service.BusinessLogic
{
    public class BaseLogic<T> : IBaseLogic<T> where T : class
    {
        protected DataContext DbContext { get; set; }

        public BaseLogic(DataContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> CreateAsync(T entity)
        {
            var result = DbContext.Set<T>().Add(entity);
            await DbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task UpdateAsync(T entity)
        {
            DbContext.Set<T>().Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            T? entity = await DbContext.Set<T>().FindAsync(id);
            if (entity is null)
            {
                return false;
            }

            DbContext.Set<T>().Remove(entity);
            var result = await DbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> EntityExists(int id)
        {
            return await DbContext.Set<T>().FindAsync(id) != null;
        }

    }
}
