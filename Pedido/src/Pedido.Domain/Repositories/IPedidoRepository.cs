using Pedido.Domain.Entities;
using Pedido.Domain.ValueObjects;
using Pedido.Domain.Shared.Repositories;


namespace Pedido.Domain.Repositories
{
    /// <summary>
    /// Interface para o repositório de pedidos.
    /// </summary>
    public interface IPedidoRepository : IRepository<Entities.Pedido>
    {
        /// <summary>
        /// Obtém um pedido pelo seu identificador, incluindo seus itens.
        /// </summary>
        /// <param name="id">O identificador do pedido</param>
        /// <returns>O pedido encontrado ou null se não existir</returns>
        Task<Entities.Pedido?> GetByIdWithItemsAsync(Guid id);

        /// <summary>
        /// Obtém pedidos por cliente, com suporte a paginação.
        /// </summary>
        /// <param name="customerId">O identificador do cliente</param>
        /// <param name="pageNumber">O número da página, começando em 1</param>
        /// <param name="pageSize">O tamanho da página</param>
        /// <returns>Uma coleção com os pedidos encontrados</returns>
        Task<IEnumerable<Entities.Pedido>> GetByClienteIdAsync(Guid customerId, int pageNumber = 1, int pageSize = 10);

        /// <summary>
        /// Obtém pedidos por status, com suporte a paginação.
        /// </summary>
        /// <param name="status">O status dos pedidos</param>
        /// <param name="pageNumber">O número da página, começando em 1</param>
        /// <param name="pageSize">O tamanho da página</param>
        /// <returns>Uma coleção com os pedidos encontrados</returns>
        Task<IEnumerable<Entities.Pedido>> GetByStatusAsync(PedidoStatus status, int pageNumber = 1, int pageSize = 10);

        /// <summary>
        /// Obtém pedidos com filtros e paginação, retornando também o total de registros.
        /// </summary>
        /// <param name="pageNumber">O número da página, começando em 1</param>
        /// <param name="pageSize">O tamanho da página</param>
        /// <param name="customerId">Filtro opcional por cliente</param>
        /// <param name="status">Filtro opcional por status</param>
        /// <returns>Uma tupla com os pedidos encontrados e o total de registros</returns>
        Task<(IEnumerable<Entities.Pedido> Pedidos, int TotalCount)> GetPedidosAsync(int pageNumber, int pageSize, Guid? clienteId = null, PedidoStatus? status = null);

        /// <summary>
        /// Verifica se um cliente possui pedidos associados.
        /// </summary>
        /// <param name="customerId">O identificador do cliente</param>
        /// <param name="cancellationToken">Token de cancelamento da operação</param>
        /// <returns>True se o cliente possui pedidos, False caso contrário</returns>
        Task<bool> CustomerHasPedidosAsync(Guid customerId, CancellationToken cancellationToken);
    }
}
