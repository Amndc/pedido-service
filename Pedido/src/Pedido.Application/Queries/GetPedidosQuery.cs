using MediatR;
using Pedido.Application.DTOs;

namespace Pedido.Application.Queries
{
    public class GetPedidosQuery : IRequest<GetPedidosQueryResult>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid? ClienteId { get; set; }
        public string? Status { get; set; }
    }
    public class GetPedidosQueryResult
    {
        public List<PedidoListItemDto> Pedidos { get; set; } = new();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }

    public class PedidoItem
    {
        public Guid Id { get; set; }
        public Guid? ClienteId { get; set; }
        public string? ClienteName { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ItemsCount { get; set; }
    }

}
