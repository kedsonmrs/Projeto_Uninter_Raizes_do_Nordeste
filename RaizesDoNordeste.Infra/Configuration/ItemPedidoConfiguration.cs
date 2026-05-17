using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesDoNordeste.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaizesDoNordeste.Infrastructure.Configuration
{
    public class ItemPedidoConfiguration : IEntityTypeConfiguration<ItemPedido>
    {
        public void Configure(EntityTypeBuilder<ItemPedido> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.PrecoUnitario).HasPrecision(18, 2);

            builder.Ignore(i => i.SubTotal);

            builder.HasOne(i => i.Pedido)
                   .WithMany(p => p.Itens)
                   .HasForeignKey(i => i.PedidoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Produto)
                   .WithMany(p => p.ItensPedidos)
                   .HasForeignKey(i => i.ProdutoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
