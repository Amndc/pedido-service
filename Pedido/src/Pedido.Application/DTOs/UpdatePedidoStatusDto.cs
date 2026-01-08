using System.ComponentModel.DataAnnotations;

namespace Pedido.Application.DTOs
{
    /// <summary>
    /// DTO para atualização do status de um pedido
    /// </summary>
    public class UpdatePedidoStatusDto
    {
        /// <summary>
        /// Novo status do pedido
        /// </summary>    /// <example>Processing</example>
        [Required(ErrorMessage = "O status é obrigatório")]
        [EnumDataType(typeof(PedidoStatusEnum), ErrorMessage = "Status inválido")]
        public required string Status { get; set; }
    }

    /// <summary>
    /// Status possíveis para um pedido
    /// </summary>
    public enum PedidoStatusEnum
    {
        /// <summary>
        /// Pedido pendente de processamento
        /// </summary>
        Pending,

        /// <summary>
        /// Pedido em processamento na cozinha
        /// </summary>
        Processing,

        /// <summary>
        /// Pedido pronto para retirada
        /// </summary>
        Ready,

        /// <summary>
        /// Pedido concluído e entregue ao cliente
        /// </summary>
        Completed,    /// <summary>
                      /// Pedido cancelado
                      /// </summary>
        Cancelled,

        /// <summary>
        /// Pedido pago
        /// </summary>
        Paid,

        /// <summary>
        /// Aguardando confirmação de pagamento
        /// </summary>
        AwaitingPayment
    }
}
