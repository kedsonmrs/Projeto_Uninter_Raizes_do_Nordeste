using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories
{
    public class FidelidadeRepository : BaseRepository<PontoFidelidade, int>, IFedilidadeRepository
    {
        public FidelidadeRepository(RaizesDoNordesteDbContext context) : base(context) { }


        public async Task AddTransacaoAsync(TransacaoFidelidade transacao)
        {
            await _context.TransacoesFidelidade.AddAsync(transacao);
        }

        public async Task<PontoFidelidade?> GetByUsuarioIdAsync(Guid usuarioId)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(p => p.UsuarioId.Equals(usuarioId));
        }

        public async Task<IEnumerable<TransacaoFidelidade>> GetTransacoesAsync(Guid usuarioId)
        {
            return await _context.TransacoesFidelidade.AsNoTracking()
                .Where(t => t.UsuarioId.Equals(usuarioId))
                .OrderByDescending(t => t.CriadoEm)
                .ToListAsync();
        }
    }
}
