using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Infrastructure.Configuration
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Observacao).HasMaxLength(500);

            builder.Property(p => p.Total).HasPrecision(18, 2);

            builder.HasOne(p => p.Usuario)
                   .WithMany(u => u.Pedidos)
                   .HasForeignKey(p => p.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Unidade)
                   .WithMany(u => u.Pedidos)
                   .HasForeignKey(p => p.UnidadeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
