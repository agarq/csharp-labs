using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Catalog.Repositories;
using Catalog.Entities;
using Catalog.Dtos;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Controllers
{
    // GET /items

    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        //this change is to implement dependency injection
        //private readonly InMemItemsRepository repository;
        private readonly IItemsRepository repository;

        public ItemsController(IItemsRepository repo)
        {
            //this change is to implement dependency injection
            //repository = new InMemItemsRepository();
            this.repository = repo;
        }

        // GET /Items
        //[HttpGet]
        /*public IEnumerable<Item> GetItems()
        {
            var items = repository.GetItems();
            return items;
        }*/
        //Changed for the next one, to start using Dto
        [HttpGet]
        public async Task <IEnumerable<ItemDto>> GetItemsAsync()
        {
            /* comment this to use the Extensions
            var items = repository.GetItems().Select(item => new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                CreatedDate = item.CreatedDate

            });*/
            var items = (await repository.GetItemsAsync())
                        .Select(item => item.AsDTo());
            return items;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id) //action result so it allows us to return the NotFound type
        {
            var item = await repository.GetItemAsync(id);

            if (item is null)
                return NotFound();

            return item.AsDTo();

        }

        // POST /Items
        [HttpPost]
        public async  Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto){
            Item item = new(){
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await repository.CreateItemAsync(item);

            //now we return the item created and a header with information
            return CreatedAtAction(nameof(GetItemAsync), new {id = item.Id}, item.AsDTo());
        }


        // PUT /Items/id
        [HttpPut("{id}")]
        public async  Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto){
            var existingItem = await repository.GetItemAsync(id);

            if(existingItem is null){
                return NotFound();
            }

            //we're taking the existing item and we create a copy of it WITH name and price updated
            //updated item is just a copy of existing item
            Item updatedItem = existingItem with{
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            await repository.UpdateItemAsync(updatedItem);

            return NoContent();
        }

        // DELETE/Items
        [HttpDelete("{id}")]
        public async  Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = await repository.GetItemAsync(id);

            if(existingItem is null){
                return NotFound();
            }

            await repository.DeleteItemAsync(id);

            return NoContent();
        }
    }

}