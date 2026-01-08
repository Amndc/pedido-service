using MediatR;

namespace Pedido.Application.Commands
{
    public class UpdatePedidoStatusCommand : IRequest<UpdatePedidoStatusCommandResult>
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }

    public class UpdatePedidoStatusCommandResult
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool NotificationSent { get; set; }
    }

}
