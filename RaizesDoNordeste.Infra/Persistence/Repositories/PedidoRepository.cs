using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enum;
using RaizesDoNordeste.Domain.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories
{
    public class PedidoRepository : BaseRepository<Pedido, int>, IPedidoRepository
    {
        public PedidoRepository(RaizesDoNordesteDbContext context) : base(context)
        {
            
        }

        public async Task<IEnumerable<Pedido>> GetAllAsync(CanalPedido? canalPedido = null, StatusPedido? status = null, int? unidadeId = null, int page = 1, int limite = 10)
        {
            var query = _dbSet.AsNoTracking().AsQueryable();

            if (canalPedido.HasValue)
            {
                query = query.Where(p => p.CanalPedido == canalPedido.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value);
            }

            if (unidadeId.HasValue)
            {
                query = query.Where(p => p.UnidadeId == unidadeId.Value);
            }

            return await query
                .OrderByDescending(p => p.CriadoEm)
                .Skip((page - 1) * limite)
                .Take(limite)
                .ToListAsync();
        }

        public async Task<Pedido?> GetByIdComItensAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .Include(p => p.Pagamento)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pedido>> GetByUsuarioAsync(Guid usuarioId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.UsuarioId.Equals(usuarioId))
                .OrderByDescending(p => p.CriadoEm)
                .ToListAsync();
        }
    }
}
