using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Infrastructure.Configuration
{
    public class PagamentoConfiguration : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ReferenciaExterna).HasMaxLength(100);
            builder.Property(p => p.MensagemRetorno).HasMaxLength(250);
            builder.Property(p => p.Valor).HasPrecision(18, 2);

            builder.HasOne(p => p.Pedido)
                   .WithOne(ped => ped.Pagamento)
                   .HasForeignKey<Pagamento>(p => p.PedidoId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
