using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Infrastructure.Configuration
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).IsRequired().HasMaxLength(150);
            builder.Property(p => p.Descricao).HasMaxLength(500);
            builder.Property(p => p.Categoria).IsRequired().HasMaxLength(50);

            builder.Property(p => p.Preco).HasPrecision(18, 2);

            builder.HasOne(p => p.Unidade)
                   .WithMany(u => u.Produtos)
                   .HasForeignKey(p => p.UnidadeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
