using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pedido.Infrastructure.Mappings
{
    /// <summary>
    /// Configuração do mapeamento de Order para o EF Core.
    /// </summary>
    public class PedidoMapping : IEntityTypeConfiguration<Domain.Entities.Pedido>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Pedido> builder)
        {
            builder.ToTable("Pedidos");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(o => o.CreatedAt)
                .IsRequired();

            builder.Property(o => o.UpdatedAt);

            builder.Property(o => o.PreferenceId)
                .HasMaxLength(100)
                .IsRequired(false);        // Relacionamento com Customer (opcional para pedidos anônimos)
            builder.HasOne(o => o.Cliente)
                .WithMany()
                .HasForeignKey(o => o.ClienteId)
                .IsRequired(false) // Permite CustomerId null para pedidos anônimos
                .OnDelete(DeleteBehavior.Restrict);// Configuração de OrderItem como owned type em uma coleção
            builder.OwnsMany(o => o.Items, ownedBuilder =>
            {
                ownedBuilder.ToTable("OrderItems");

                // Configurar o Id como GUID usando uma sombra property
                ownedBuilder.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();
                ownedBuilder.HasKey("Id");

                // Propriedades do OrderItem
                ownedBuilder.Property(i => i.ProdutoId)
                    .IsRequired();

                ownedBuilder.Property(i => i.ProdutoNome)
                    .IsRequired()
                    .HasMaxLength(100);

                ownedBuilder.Property(i => i.UnitPrice)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                ownedBuilder.Property(i => i.Quant)
                    .IsRequired();

                // Configurar o pedido pai
                ownedBuilder.WithOwner()
                    .HasForeignKey("OrderId");
            });
        }
    }
}
