using System.Threading.Tasks;

namespace News.Vampire.Service.BusinessLogic.Interfaces
{
    public interface IBaseLogic<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<T?> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task<bool> EntityExists(int id);
    }
}
