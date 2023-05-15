using News.Vampire.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace News.Vampire.Services
{
    public class MockedSourceDataStore : IDataStore<Source>
    {
        readonly List<Source> items;

        public MockedSourceDataStore()
        {
            items = new List<Source>()
            {
                new Source { Id = Guid.NewGuid().ToString(), GroupId = "1",  Text = "First Source", Url = "https://www.apple.com/", Sort = 3},
                new Source { Id = Guid.NewGuid().ToString(), GroupId = "2",  Text = "Second Source", Url = "https://www.apple.com/", Sort = 2},
                new Source { Id = Guid.NewGuid().ToString(), GroupId = "3",  Text = "Third Source", Url = "https://www.apple.com/", Sort = 1},
            };
        }

        public async Task<bool> AddAsync(Source item)
        {
            items.Add(item);

            return await Task.FromResult(true);

        }

        public async Task<bool> DeleteAsync(string id)
        {
            var oldItem = items.Where((Source arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Source> GetAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Source>> GetAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public async Task<bool> UpdateAsync(Source item)
        {
            var oldItem = items.Where((Source arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);

        }
    }
}
