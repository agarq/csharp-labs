using System;
using System.Linq;
using Catalog.Entities;
using System.Collections.Generic;

namespace Catalog.Repositories
{
    /*
    public class InMemItemsRepository : IItemsRepository
    {
        //define very simple list of items
        //should not change after we construct this repository object
        private readonly List<Item> items = new()
        {
            new Item { Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreatedDate = DateTimeOffset.UtcNow},
            new Item { Id = Guid.NewGuid(), Name = "Iron Sword", Price = 20, CreatedDate = DateTimeOffset.UtcNow},
            new Item { Id = Guid.NewGuid(), Name = "Bronze Shield", Price = 8, CreatedDate = DateTimeOffset.UtcNow}
        };

        public IEnumerable<Item> GetItems()
        {
            return items;
        }

        public Item GetItemAsync(Guid id)
        {
            //return items.Where(item => item.Id == id).SingleOrDefault();
            return items.SingleOrDefault(item => item.Id == id); //better way in this case to return a single item.
        }

        public void CreateItemAsync(Item item)
        {
             items.Add(item);
        }

        public void UpdateItemAsync(Item item)
        {
            var index = items.FindIndex(existingItem => existingItem.Id == item.Id);
            items[index] = item;
        }

        public void DeleteItemAsync(Guid id)
        {
            var index = items.FindIndex(existingItem => existingItem.Id == id);
            items.RemoveAt(index);
        }
    }
*/

}