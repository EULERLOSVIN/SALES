using SALES.Application.DTOs;

namespace SALES.Application.Common.Events
{
    // Usamos record para asegurar que el mensaje sea inmutable
    public record SaleCreatedEvent
    {
        public int IdSale { get; init; }
        public int IdCart { get; init; }
        public List<SaleItemDto> Items { get; init; } = new();
    }
}