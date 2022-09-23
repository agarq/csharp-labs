using Catalog.Dtos;
using Catalog.Entities;
namespace Catalog
{
    //for extension methods, we use static class
    public static class Extensions{
        public static ItemDto AsDTo(this Item item)
        {
            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                CreatedDate = item.CreatedDate
            };
        }
    }
}