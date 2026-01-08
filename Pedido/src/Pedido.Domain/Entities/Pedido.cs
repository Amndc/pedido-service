using Pedido.Domain.Custumer.Entities;
using Pedido.Domain.Exceptions;
using Pedido.Domain.Shared.Entities;
using Pedido.Domain.ValueObjects;

namespace Pedido.Domain.Entities
{
    /// <summary>
    /// Representa um pedido no sistema.
    /// </summary>
    public class Pedido : Entity

    {    /// <summary>
         /// Cliente que realizou o pedido.
         /// </summary>
        public Cliente? Cliente { get; private set; }   

        /// <summary>
        /// ID do cliente que realizou o pedido. Null para pedidos anônimos.
        /// </summary>
        public Guid? ClienteId { get; private set; }

        /// <summary>
        /// Itens do pedido.
        /// </summary>
        public IReadOnlyCollection<PedidoItem> Items => _items.AsReadOnly();
        private readonly List<PedidoItem> _items;

        /// <summary>
        /// Status do pedido.
        /// </summary>
        public PedidoStatus Status { get; private set; } 
        
        /// <summary>
        /// Valor total do pedido.
        /// </summary>
        public decimal TotalPrice { get; private set; }

        /// <summary>
        /// QR Code gerado para pagamento do pedido.
        /// </summary>
        public string? QrCode { get; private set; }

        /// <summary>
        /// ID da preferência no Mercado Pago.
        /// </summary>
        public string? PreferenceId { get; private set; }

        /// <summary>
        /// Construtor privado para uso do EF Core.
        /// </summary>
        private Pedido() : base()
        {
            _items = new List<PedidoItem>();
            Status = PedidoStatus.Pending;
            TotalPrice = 0;
        }    /// <summary>
             /// Construtor interno para criação de pedido.
             /// </summary>
             /// <param name="customerId">ID do cliente que está realizando o pedido (null para pedidos anônimos).</param>
             /// <param name="items">Itens do pedido.</param>
        private Pedido(Guid? customerId, List<PedidoItem> items) : base()
        {
            ClienteId = customerId;
            _items = items ?? new List<PedidoItem>();
            Status = PedidoStatus.Pending;
            CalculateTotalPrice();
        }    /// <summary>
             /// Factory method para criar um novo pedido com validação de todos os campos.
             /// </summary>
             /// <param name="customerId">ID do cliente que está realizando o pedido (null para pedidos anônimos).</param>
             /// <param name="items">Itens do pedido.</param>
             /// <returns>Uma nova instância de Order com os dados validados.</returns>
             /// <exception cref="OrderDomainException">Lançada quando algum dos campos é inválido.</exception>
             /// 
        public static Pedido Create(Guid? customerId, List<PedidoItem> items)
        {
            // Validar que quando customerId é informado, não pode ser Guid.Empty
            if (customerId.HasValue && customerId.Value == Guid.Empty)
                throw new PedidoDomainException("Quando informado, o ID do cliente deve ser um GUID válido");

            if (items == null || !items.Any())
                throw new PedidoDomainException("O pedido deve ter pelo menos um item");

            return new Pedido(customerId, items);
        }

        /// <summary>
        /// Adiciona um item ao pedido.
        /// </summary>
        /// <param name="item">Item a ser adicionado.</param>
        /// <exception cref="PedidoDomainException">Lançada quando o pedido não está mais em estado pendente.</exception>
        public void AddItem(PedidoItem item)
        {
            if (Status != PedidoStatus.Pending)
                throw new PedidoDomainException("Não é possível adicionar itens a um pedido que não está pendente");

            if (item == null)
                throw new PedidoDomainException("O item não pode ser nulo");

            _items.Add(item);
            CalculateTotalPrice();
            SetUpdatedAt();
        }

        /// <summary>
        /// Remove um item do pedido pelo ID do produto.
        /// </summary>
        /// <param name="productId">ID do produto a ser removido.</param>
        /// <returns>true se o item foi removido; false caso contrário.</returns>
        /// <exception cref="OrderDomainException">Lançada quando o pedido não está mais em estado pendente.</exception>
        public bool RemoveItem(Guid produtoId)
        {
            if (Status != PedidoStatus.Pending)
                throw new PedidoDomainException("Não é possível remover itens de um pedido que não está pendente");

            var item = _items.FirstOrDefault(i => i.ProdutoId == produtoId);
            if (item == null)
                return false;

            var removed = _items.Remove(item);
            if (removed)
            {
                CalculateTotalPrice();
                SetUpdatedAt();
            }
            return removed;
        }

        /// <summary>
        /// Atualiza o status do pedido.
        /// </summary>
        /// <param name="status">Novo status do pedido.</param>
        /// <exception cref="OrderDomainException">Lançada quando a transição de status não é permitida.</exception>
        public void UpdateStatus(PedidoStatus status)
        {
            // Validar transições de status
            if (!IsValidStatusTransition(Status, status))
                throw new PedidoDomainException($"A transição do status {Status} para {status} não é permitida");

            Status = status;
            SetUpdatedAt();
        }

        /// <summary>
        /// Define o QR Code do pedido.
        /// </summary>
        /// <param name="qrCode">Código QR gerado para pagamento.</param>
        public void SetQrCode(string qrCode)
        {
            if (string.IsNullOrWhiteSpace(qrCode))
                throw new PedidoDomainException("O QR Code não pode ser vazio");

            QrCode = qrCode;
            SetUpdatedAt();
        }

        /// <summary>
        /// Define o ID da preferência do Mercado Pago para o pedido.
        /// </summary>
        /// <param name="preferenceId">ID da preferência no Mercado Pago.</param>
        public void SetPreferenceId(string preferenceId)
        {
            if (string.IsNullOrWhiteSpace(preferenceId))
                throw new PedidoDomainException("O ID da preferência não pode ser vazio");

            PreferenceId = preferenceId;
            SetUpdatedAt();
        }

        /// <summary>
        /// Verifica se a transição de status é válida.
        /// </summary>
        /// <param name="currentStatus">Status atual.</param>
        /// <param name="newStatus">Novo status.</param>
        /// <returns>true se a transição é válida; false caso contrário.</returns>
        private bool IsValidStatusTransition(PedidoStatus currentStatus, PedidoStatus newStatus)
        {
            return (currentStatus, newStatus) switch
            {
                // Transições básicas do fluxo principal
                (PedidoStatus.Pending, PedidoStatus.Processing) => true,
                (PedidoStatus.Processing, PedidoStatus.Ready) => true,
                (PedidoStatus.Ready, PedidoStatus.Completed) => true,
                (PedidoStatus.Pending, PedidoStatus.Cancelled) => true,
                (PedidoStatus.Processing, PedidoStatus.Cancelled) => true,

                // Transições de pagamento - NOVO FLUXO
                (PedidoStatus.Pending, PedidoStatus.AwaitingPayment) => true,  // Checkout gera QR Code
                (PedidoStatus.AwaitingPayment, PedidoStatus.Paid) => true,     // Confirmação de pagamento
                (PedidoStatus.AwaitingPayment, PedidoStatus.Cancelled) => true, // Cancelamento durante espera
                (PedidoStatus.Paid, PedidoStatus.Processing) => true,          // Envio para cozinha

                // Transições do fluxo antigo (manter compatibilidade)
                (PedidoStatus.Pending, PedidoStatus.Paid) => true,

                // Transição reversa permitida (ex: problemas na cozinha, falta de ingredientes)
                (PedidoStatus.Processing, PedidoStatus.Pending) => true,

                // Status igual, permitido (idempotência)
                var (current, next) when current == next => true,

                // Qualquer outra transição é inválida
                _ => false
            };
        }

        /// <summary>
        /// Calcula o valor total do pedido com base nos itens.
        /// </summary>
        private void CalculateTotalPrice()
        {
            TotalPrice = _items.Sum(item => item.SubTotal);
        }
    }
}

