using System;
using Catalog.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Repositories
{
    public interface IItemsRepository
    {
        // we changed to tasks because this will be asynchronous
        Task<Item> GetItemAsync(Guid id);
        Task<IEnumerable<Item>> GetItemsAsync();
        Task CreateItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(Guid id);

    }
}