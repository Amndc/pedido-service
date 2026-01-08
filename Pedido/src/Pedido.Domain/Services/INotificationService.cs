
using Pedido.Domain.ValueObjects;

namespace Pedido.Domain.Services
{
    /// <summary>
    /// Interface para serviço de notificação de mudanças de status de pedidos.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Notifica o cliente sobre uma mudança de status no pedido.
        /// </summary>
        /// <param name="Pedido">Pedido atualizado</param>
        /// <param name="previousStatus">Status anterior do pedido</param>
        /// <returns>Task representando a operação assíncrona</returns>
        Task NotifyPedidoStatusChangeAsync(Entities.Pedido pedido, PedidoStatus previousStatus);

        /// <summary>
        /// Notifica o cliente que o pedido está pronto para retirada.
        /// </summary>
        /// <param name="Pedido">Pedido pronto</param>
        /// <returns>Task representando a operação assíncrona</returns>
        Task NotifyPedidoReadyAsync(Entities.Pedido pedido);
    }
}
