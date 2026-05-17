using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Infrastructure.Configuration
{
    public class EstoqueConfiguration : IEntityTypeConfiguration<Estoque>
    {
        public void Configure(EntityTypeBuilder<Estoque> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Unidades)
                   .WithMany(u => u.Estoques)
                   .HasForeignKey(e => e.UnidadeId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(e => e.Produtos)
                   .WithOne(p => p.Estoque)
                   .HasForeignKey<Estoque>(e => e.ProdutoId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
