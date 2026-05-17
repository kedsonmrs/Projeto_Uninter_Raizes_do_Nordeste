using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Infrastructure.Configuration
{
    public class PontoFidelidadeConfiguration : IEntityTypeConfiguration<PontoFidelidade>
    {
        public void Configure(EntityTypeBuilder<PontoFidelidade> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
