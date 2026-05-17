using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Infrastructure.Configuration
{
    public class TransacaoFidelidadeConfiguration : IEntityTypeConfiguration<TransacaoFidelidade>
    {
        public void Configure(EntityTypeBuilder<TransacaoFidelidade> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Descricao).HasMaxLength(200);

            builder.HasOne(t => t.Usuario)
                   .WithMany()
                   .HasForeignKey(t => t.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Pedido)
                   .WithMany()
                   .HasForeignKey(t => t.PedidoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
