using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories
{
    public class ProdutoRepository : BaseRepository<Produto, int>, IProdutoRepository
    {
        public ProdutoRepository(RaizesDoNordesteDbContext context) : base(context) { }

        public async Task<IEnumerable<Produto>> GetByUnidadeAsync(int unidadeId, bool somenteDisponiveis = true)
        {
            var query = _dbSet.AsNoTracking().Where(p => p.UnidadeId.Equals(unidadeId));

            if (somenteDisponiveis)
            {
                query = query.Where(p => p.Disponivel);
            }

            return await query
                .OrderBy(p => p.Categoria)
                .ThenBy(p => p.Nome)
                .ToListAsync();
        }
    }
}
