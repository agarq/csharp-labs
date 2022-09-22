using System;

namespace Catalog.Entities
{
    public record Item
    {
        public Guid Id { get; init; } //init: only allows setting a value during its initialization, so it's immutable

        public string Name { get; init; }

        public decimal Price { get; init; }

        public DateTimeOffset CreatedDate { get; init; }
    }
}