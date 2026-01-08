using MediatR;
using Pedido.Application.DTOs;

namespace Pedido.Application.Queries
{
    public class GetPedidoByIdQuery : IRequest<GetPedidoByIdQueryResult>
    {
        public Guid Id { get; set; }
    }
    public class GetPedidoByIdQueryResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public PedidoDto? Order { get; set; }
    }
}
