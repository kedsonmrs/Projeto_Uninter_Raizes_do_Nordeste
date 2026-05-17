using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Infrastructure.Configuration
{
    public class UnidadeConfiguration : IEntityTypeConfiguration<Unidade>
    {
        public void Configure(EntityTypeBuilder<Unidade> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Nome).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Endereco).IsRequired().HasMaxLength(250);
            builder.Property(u => u.Telefone).HasMaxLength(20);
        }
    }
}
