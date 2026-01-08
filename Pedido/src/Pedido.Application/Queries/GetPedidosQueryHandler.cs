using MediatR;
using Microsoft.Extensions.Logging;
using Pedido.Application.DTOs;
using Pedido.Domain.Repositories;
using Pedido.Domain.ValueObjects;

namespace Pedido.Application.Queries
{
    public class GetPedidosQueryHandler :  IRequestHandler<GetPedidosQuery, GetPedidosQueryResult>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ILogger<GetPedidosQueryHandler> _logger;

        public GetPedidosQueryHandler(IPedidoRepository pedidoRepository, ILogger<GetPedidosQueryHandler> logger)
        {
            _pedidoRepository = pedidoRepository;
            _logger = logger;
        }

        public async Task<GetPedidosQueryResult> Handle(GetPedidosQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Parse status if provided
                PedidoStatus? status = null;
                if (!string.IsNullOrWhiteSpace(request.Status) &&
                    Enum.TryParse<PedidoStatus>(request.Status, true, out var parsedStatus))
                {
                    status = parsedStatus;
                }

                // Ensure valid pagination values
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 10;
                if (request.PageSize > 100) request.PageSize = 100;

                // Get orders with pagination and filtering
                var (pedidos, totalCount) = await _pedidoRepository.GetPedidosAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.ClienteId,
                    status);            // Map to DTO
                var pedidoItems = pedidos.Select(o => new PedidoListItemDto
                {
                    Id = o.Id,
                    ClienteId = o.ClienteId,
                    ClienteNome = o.Cliente?.Nome, // Null para pedidos anônimos
                    TotalPrice = o.TotalPrice,
                    Status = o.Status.ToString(),
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt,
                    ItemsCount = o.Items.Count
                }).ToList();

                // Build and return result
                return new GetPedidosQueryResult
                {
                    Pedidos = pedidoItems,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = totalCount
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lista de pedidos");
                // Return empty result to avoid breaking the API
                return new GetPedidosQueryResult
                {
                    Pedidos = new List<PedidoListItemDto>(),
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = 0
                };
            }
        }
    }
}
