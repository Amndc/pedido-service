using Pedido.Domain.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Pedido.Domain.Entities;
using Pedido.Domain.Custumer.Entities;
using Pedido.Domain.ValueObjects;


namespace Pedido.Infrastructure.Data
{
    public class PedidoDbContext : DbContext
    {
        public DbSet<Domain.Entities.Pedido> Pedidos { get; set; }
        public DbSet<PedidoItem> PedidoItems { get; set; }

        public PedidoDbContext(DbContextOptions<PedidoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade Order
            modelBuilder.Entity<Domain.Entities.Pedido>(entity =>
            {
                entity.Property(e => e.QrCode)
                    .HasMaxLength(1000);

                entity.Property(e => e.Status)
                    .HasConversion<string>();

                // Configurar as propriedades de data para usar o fuso horário do Brasil
                entity.Property(e => e.CreatedAt)
                    .HasConversion(
                        v => v.ToUniversalTime(),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc).ToLocalTime()
                    );

                entity.Property(e => e.UpdatedAt)
                    .HasConversion(
                        v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc).ToLocalTime() : (DateTime?)null
                    );
            });

            // Configurar as propriedades de data para todas as entidades que herdam de Entity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(Entity).IsAssignableFrom(entityType.ClrType))
                {
                    var createdAtProperty = entityType.FindProperty(nameof(Entity.CreatedAt));
                    if (createdAtProperty != null && createdAtProperty.ClrType == typeof(DateTime))
                    {
                        createdAtProperty.SetValueConverter(
                            new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
                                v => v.ToUniversalTime(),
                                v => DateTime.SpecifyKind(v, DateTimeKind.Utc).ToLocalTime()
                            )
                        );
                    }

                    var updatedAtProperty = entityType.FindProperty(nameof(Entity.UpdatedAt));
                    if (updatedAtProperty != null && updatedAtProperty.ClrType == typeof(DateTime?))
                    {
                        updatedAtProperty.SetValueConverter(
                            new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime?, DateTime?>(
                                v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc).ToLocalTime() : (DateTime?)null
                            )
                        );
                    }
                }
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PedidoDbContext).Assembly);
        }
    }
}
