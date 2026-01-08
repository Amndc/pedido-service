using Pedido.Domain.Exceptions;
using Pedido.Domain.Shared.ValueObjects;

namespace Pedido.Domain.ValueObjects
{
    public sealed class PedidoItem : ValueObject
    {
        /// <summary>
        /// ID do item no pedido.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// ID do produto.
        /// </summary>
        public Guid ProdutoId { get; private set; }

        /// <summary>
        /// Nome do produto no momento do pedido.
        /// </summary>
        public string ProdutoNome { get; private set; }

        /// <summary>
        /// Preço unitário do produto no momento do pedido.
        /// </summary>
        public decimal UnitPrice { get; private set; }

        /// <summary>
        /// Quantidade do produto.
        /// </summary>
        public int Quant { get; private set; }

        /// <summary>
        /// Valor total do item (quantidade * preço unitário).
        /// </summary>
        public decimal SubTotal => Quant * UnitPrice;

        /// <summary>
        /// Construtor privado para uso do EF Core.
        /// </summary>
        private PedidoItem()
        {
            ProdutoNome = string.Empty;
        }

        /// <summary>
        /// Construtor interno para criação de um item de pedido.
        /// </summary>
        private PedidoItem(Guid id, Guid produtoId, string produtoNome, decimal unitPrice, int quant)
        {
            Id = id;
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            UnitPrice = unitPrice;
            Quant = quant;
        }

        /// <summary>
        /// Factory method para criar um novo item de pedido com validação de todos os campos.
        /// </summary>
        /// <param name="productId">ID do produto.</param>
        /// <param name="productName">Nome do produto.</param>
        /// <param name="unitPrice">Preço unitário do produto.</param>
        /// <param name="quantity">Quantidade do produto.</param>
        /// <returns>Uma nova instância de OrderItem com os dados validados.</returns>
        /// <exception cref="OrderDomainException">Lançada quando algum dos campos é inválido.</exception>
        public static PedidoItem Create(Guid produtoId, string produtoNome, decimal unitPrice, int quant)
        {
            if (produtoId == Guid.Empty)
                throw new PedidoDomainException("O ID do produto é obrigatório");

            if (string.IsNullOrWhiteSpace(produtoNome))
                throw new PedidoDomainException("O nome do produto é obrigatório");

            if (unitPrice <= 0)
                throw new PedidoDomainException("O preço unitário deve ser maior que zero");

            if (quant <= 0)
                throw new PedidoDomainException("A quantidade deve ser maior que zero");

            return new PedidoItem(Guid.NewGuid(), produtoId, produtoNome, unitPrice, quant);
        }

        /// <summary>
        /// Retorna um novo OrderItem com a quantidade atualizada.
        /// </summary>
        /// <param name="quantity">Nova quantidade.</param>
        /// <returns>Um novo OrderItem com a quantidade atualizada.</returns>
        /// <exception cref="OrderDomainException">Lançada quando a quantidade é inválida.</exception>
        public PedidoItem WithQuantity(int quant)
        {
            if (quant <= 0)
                throw new PedidoDomainException("A quantidade deve ser maior que zero");

            return new PedidoItem(Id, ProdutoId, ProdutoNome, UnitPrice, quant);
        }    /// <summary>
             /// Retorna os valores que compõem este value object.
             /// </summary>
        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return ProdutoId;
            yield return ProdutoNome;
            yield return UnitPrice;
            yield return Quant;
        }

    }
}
