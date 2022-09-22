using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Catalog.Repositories;
using Catalog.Entities;

namespace Catalog.Controllers
{
    // GET /items

    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly InMemItemsRepository repository;

        public ItemsController()
        {
            repository = new InMemItemsRepository();
        }

        // GET /items
        [HttpGet]
        public IEnumerable<Item> GetItems()
        {
            var items = repository.GetItems();
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<Item> GetItem(Guid id) //action result so it allows us to return the NotFound type
        {
            var item = repository.GetItem(id);

            if (item is null)
                return NotFound();

            return item;

        }
    }

}