using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using System.Reflection;

namespace RaizesDoNordeste.Infrastructure.Persistence
{
    public class RaizesDoNordesteDbContext : DbContext
    {
        public RaizesDoNordesteDbContext()
        {
            
        }

        public RaizesDoNordesteDbContext(DbContextOptions<RaizesDoNordesteDbContext> options) : base(options)
        {
            
        }

        public DbSet<Estoque> Estoques { get; set; }
        public DbSet<ItemPedido> ItemPedidos { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PontoFidelidade> PontosFidelidade { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<TransacaoFidelidade> TransacoesFidelidade { get; set; }
        public DbSet<Unidade> Unidades { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
