using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Catalog.Repositories;
using Catalog.Entities;
using Catalog.Dtos;
using System.Linq;

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
        public IEnumerable<ItemDto> GetItems()
        {
            /* comment this to use the Extensions
            var items = repository.GetItems().Select(item => new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                CreatedDate = item.CreatedDate

            });*/
            var items = repository.GetItems().Select(item => item.AsDTo());
            return items;
        }


        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id) //action result so it allows us to return the NotFound type
        {
            var item = repository.GetItem(id);

            if (item is null)
                return NotFound();

            return item.AsDTo();

        }

        // POST /Items
        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto){
            Item item = new(){
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            repository.CreateItem(item);

            //now we return the item created and a header with information
            return CreatedAtAction(nameof(GetItem), new {id = item.Id}, item.AsDTo());
        }


        // PUT /Items/id
        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto){
            var existingItem = repository.GetItem(id);

            if(existingItem is null){
                return NotFound();
            }

            //we're taking the existing item and we create a copy of it WITH name and price updated
            //updated item is just a copy of existing item
            Item updatedItem = existingItem with{
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            repository.UpdateItem(updatedItem);

            return NoContent();
        }

        // DELETE/Items
        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id)
        {
            var existingItem = repository.GetItem(id);

            if(existingItem is null){
                return NotFound();
            }

            repository.DeleteItem(id);

            return NoContent();
        }
    }

}