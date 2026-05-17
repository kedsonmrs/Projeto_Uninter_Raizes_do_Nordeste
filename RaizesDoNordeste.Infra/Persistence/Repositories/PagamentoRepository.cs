using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories
{
    public class PagamentoRepository : BaseRepository<Pagamento, int>, IPagamentoRepository
    {
        public PagamentoRepository(RaizesDoNordesteDbContext context) : base(context) { }

        public async Task<Pagamento?> GetByPedidoAsync(int pedidoId)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(p => p.PedidoId.Equals(pedidoId));
        }
    }
}
