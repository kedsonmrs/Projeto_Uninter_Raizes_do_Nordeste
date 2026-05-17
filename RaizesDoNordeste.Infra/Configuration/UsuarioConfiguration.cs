using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Infrastructure.Configuration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Nome).IsRequired().HasMaxLength(150);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
            builder.Property(u => u.SenhaHash).IsRequired().HasMaxLength(255);
            builder.Property(u => u.Telefone).HasMaxLength(20);

            builder.HasOne(u => u.PontosFidelidade)
                   .WithOne(p => p.Usuario)
                   .HasForeignKey<PontoFidelidade>(p => p.UsuarioId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
