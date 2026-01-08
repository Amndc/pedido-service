using Microsoft.EntityFrameworkCore;
using Pedido.Domain.Repositories;
using Pedido.Domain.ValueObjects;
using Pedido.Infrastructure.Data;



namespace Pedido.Infrastructure.Repositories
{
    public class PedidoRepository : RepositoryBase<Domain.Entities.Pedido>, IPedidoRepository
    {
        public PedidoRepository(PedidoDbContext context) : base(context)
        {
        }

        public override async Task<Domain.Entities.Pedido?> GetByIdAsync(Guid id)
        {
            return await DbSet
                .Include(o => o.Cliente)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Domain.Entities.Pedido?> GetByIdWithItemsAsync(Guid id)
        {
            return await DbSet
                .Include(o => o.Cliente)
                .Include(o => o.Items) // Mudança: usar expressão lambda
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public override async Task<IEnumerable<Domain.Entities.Pedido>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await DbSet
                .Include(o => o.Cliente)
                .Include(o => o.Items) // Mudança: usar expressão lambda
                .OrderByDescending(o => o.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entities.Pedido>> GetByClienteIdAsync(Guid customerId, int pageNumber = 1, int pageSize = 10)
        {
            return await DbSet
                .Include(o => o.Cliente)
                .Include(o => o.Items) // Mudança: usar expressão lambda
                .Where(o => o.ClienteId == customerId)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entities.Pedido>> GetByStatusAsync(PedidoStatus status, int pageNumber = 1, int pageSize = 10)
        {
            return await DbSet
                .Include(o => o.Cliente)
                .Include(o => o.Items) // Mudança: usar expressão lambda
                .Where(o => o.Status == status)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Domain.Entities.Pedido> Pedidos, int TotalCount)> GetPedidosAsync(
            int pageNumber,
            int pageSize,
            Guid? clienteId = null,
            PedidoStatus? status = null)
        {
            var query = DbSet
                .Include(o => o.Cliente)
                .Include(o => o.Items) // Mudança: usar expressão lambda
                .AsQueryable();

            if (clienteId.HasValue)
                query = query.Where(o => o.ClienteId == clienteId);

            if (status.HasValue)
                query = query.Where(o => o.Status == status);

            var totalCount = await query.CountAsync(); var orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (orders, totalCount);
        }

        public async Task<bool> CustomerHasPedidosAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AnyAsync(o => o.ClienteId == customerId, cancellationToken);
        }
    }
}
