using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace News.Vampire.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddAsync(T item);
        Task<bool> UpdateAsync(T item);
        Task<bool> DeleteAsync(string id);
        Task<T> GetAsync(string id);
        Task<IEnumerable<T>> GetAsync(bool forceRefresh = false);
    }
}
