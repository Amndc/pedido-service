namespace Pedido.Application.DTOs
{
    /// <summary>
    /// DTO com os dados de um item do pedido
    /// </summary>
    public class PedidoItemDto
    {
        /// <summary>
        /// Identificador único do item do pedido
        /// </summary>
        public required Guid Id { get; set; }

        /// <summary>
        /// Identificador do produto
        /// </summary>
        public required Guid ProdutoId { get; set; }

        /// <summary>
        /// Nome do produto
        /// </summary>
        public required string ProdutoNome { get; set; }

        /// <summary>
        /// Preço unitário do produto
        /// </summary>
        public required decimal UnitPrice { get; set; }

        /// <summary>
        /// Quantidade solicitada do produto
        /// </summary>
        public required int Quant { get; set; }

        /// <summary>
        /// Subtotal do item (quantidade * preço unitário)
        /// </summary>
        public required decimal SubTotal { get; set; }

        /// <summary>
        /// Observações do item (exemplo: sem cebola, sem tomate, etc.)
        /// </summary>
        public string? Observation { get; set; }
    }
}
