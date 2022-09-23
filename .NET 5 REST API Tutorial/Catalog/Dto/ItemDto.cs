using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Dtos
{
    public record ItemDto
    {
        public Guid Id { get; init; }

        [Required]
        public string Name { get; init; }

        [Required]
        [Range(0, 99999)]
        public decimal Price { get; init; }

        public DateTimeOffset CreatedDate { get; init; }
    }
}
