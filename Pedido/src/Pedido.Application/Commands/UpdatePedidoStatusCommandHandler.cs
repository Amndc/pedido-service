
using Microsoft.Extensions.Logging;
using Pedido.Application.Common;
using Pedido.Domain.Repositories;
using Pedido.Domain.Services;
using Pedido.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Pedido.Application.Commands
{
    public class UpdatePedidoStatusCommandHandler
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<UpdatePedidoStatusCommandHandler> _logger;

        public UpdatePedidoStatusCommandHandler(
            IPedidoRepository orderRepository,
            INotificationService notificationService,
            ILogger<UpdatePedidoStatusCommandHandler> logger)
        {
            _pedidoRepository = orderRepository;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<UpdatePedidoStatusCommandResult> Handle(UpdatePedidoStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Atualizando status do pedido {OrderId} para {NewStatus}", request.Id, request.Status);

                // Buscar o pedido pelo ID com detalhes do cliente
                var order = await _pedidoRepository.GetByIdWithItemsAsync(request.Id);
                if (order == null)
                    throw new NotFoundException($"Pedido com ID {request.Id} não encontrado");

                // Converter a string para enum
                if (!Enum.TryParse<PedidoStatus>(request.Status, true, out var newStatus))
                    throw new ValidationException($"Status inválido: {request.Status}");

                // Guardar o status anterior para notificação
                var previousStatus = order.Status;

                // Atualizar o status do pedido
                order.UpdateStatus(newStatus);

                // Persistir a atualização
                await _pedidoRepository.UpdateAsync(order);

                // Notificar o cliente sobre a mudança de status
                await _notificationService.NotifyPedidoStatusChangeAsync(order, previousStatus);

                // Se o status foi atualizado para Ready (Pronto), enviar notificação específica
                if (newStatus == PedidoStatus.Ready)
                {
                    _logger.LogInformation("Pedido {OrderId} está pronto para retirada! Enviando notificação especial", order.Id);
                    await _notificationService.NotifyPedidoReadyAsync(order);
                }            // Retornar o resultado
                var result = new UpdatePedidoStatusCommandResult
                {
                    Id = order.Id,
                    Status = order.Status.ToString(),
                    UpdatedAt = order.UpdatedAt.Value,
                    NotificationSent = true // Indicar que a notificação foi enviada
                };

                _logger.LogInformation("Status do pedido {OrderId} atualizado com sucesso para {NewStatus}", order.Id, newStatus);
                return result;
            }
            catch (Exception ex) when (ex is not NotFoundException && ex is not ValidationException)
            {
                _logger.LogError(ex, "Erro ao atualizar status do pedido {OrderId} para {NewStatus}", request.Id, request.Status);
                throw;
            }
        }
    }
}
