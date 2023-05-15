using News.Vampire.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace News.Vampire.Services
{
    public class MockGroupDataStore : IDataStore<Group>
    {
        readonly List<Group> items;

        public MockGroupDataStore()
        {
            items = new List<Group>()
            {
                new Group { Id = "1", Name = "First Group", isActive = true },
                new Group { Id = "3", Name = "Second Group", isActive = true },
            };
        }

        public async Task<bool> AddAsync(Group item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var oldItem = items.Where((Group arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Group> GetAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Group>> GetAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public async Task<bool> UpdateAsync(Group item)
        {
            var oldItem = items.Where((Group  arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }
    }
}
