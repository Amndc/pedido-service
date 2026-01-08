using Pedido.Domain.Entities;
using MediatR;
using Pedido.Domain.Repositories;
using Pedido.Domain.ValueObjects;
using Pedido.Application.Common;
using Pedido.Domain.Custumer.Entities;

namespace Pedido.Application.Commands
{
    public class CreatePedidoCommandHandler : IRequestHandler<CreatePedidoCommand, CreatePedidoCommandResult>
    {
        private readonly IPedidoRepository _orderRepository;


        public CreatePedidoCommandHandler(IPedidoRepository orderRepository){
            _orderRepository = orderRepository;
        }
        public async Task<CreatePedidoCommandResult> Handle(CreatePedidoCommand request, CancellationToken cancellationToken)
        {
            // Criar os itens do pedido
           // var pedidoItems = new List<PedidoItem>();

            var pedidoItems = request.Items.Select(item =>
               PedidoItem.Create(
                   item.ProdutoId,
                   item.ProdutoNome,
                   item.UnitPrice,
                   item.Quant
               )
           ).ToList();

            // Criar o pedido (pode ser anônimo)
            var pedido = Domain.Entities.Pedido.Create(
                request.ClienteId,
                pedidoItems
            ); 
   
            // Persistir o pedido
            await _orderRepository.CreateAsync(pedido);

            // Retornar o resultado
            return new CreatePedidoCommandResult
            {
                Id = pedido.Id,
                ClienteId = pedido.ClienteId,
                ClienteName = request.ClienteNome, // Null para pedidos anônimos
                Status = pedido.Status.ToString(),
                TotalPrice = pedido.TotalPrice,
                CreatedAt = pedido.CreatedAt,
                Items = pedido.Items.Select(item => new CreatePedidoItemCommandResult
                {
                    Id = item.Id,
                    ProdutoId = item.ProdutoId,
                    ProdutoNome = item.ProdutoNome,
                    UnitPrice = item.UnitPrice,
                    Quant = item.Quant,
                    SubTotal = item.SubTotal
                }).ToList()
            };
        }
    }
}
