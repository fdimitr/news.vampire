using News.Vampire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using News.Vampire.Service.Protos;

namespace News.Vampire.Services
{
    public class MockItemDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public MockItemDataStore()
        {
            //items = new List<Item>()
            //{
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            //};

            items = new List<Item>(LoadItems());
        }

        public async Task<bool> AddAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        //-----------------------------------------------------------------------------
        public List<Item> LoadItems()
        {
            List<Item> result = new List<Item>();
            UserGroupRequest request = new UserGroupRequest();
            request.UserId = 1;

            var channel = new Channel("localhost:5000", ChannelCredentials.Insecure);
            UserGroupServiceGrpc.UserGroupServiceGrpcClient client = new UserGroupServiceGrpc.UserGroupServiceGrpcClient(channel);
            var userGroupsGrpc = client.GetUserGroups(request);

            if (userGroupsGrpc != null && userGroupsGrpc.Result != null)
            {
                foreach (var userGroupGrps in userGroupsGrpc.Result)
                {
                    foreach (var sourceGrpc in userGroupGrps.Sources)
                    {
                        var item = new Item
                        {
                            Id = sourceGrpc.Id.ToString(),
                            Text = sourceGrpc.Name,
                            Description = userGroupGrps.Name
                        };
                        result.Add(item);
                    }
                }
            }

            return result;
        }
    }
}