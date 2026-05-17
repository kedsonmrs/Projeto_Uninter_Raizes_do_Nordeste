using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories
{
    public class EstoqueRepository : BaseRepository<Estoque, int>, IEstoqueRepository
    {
        public EstoqueRepository(RaizesDoNordesteDbContext context) : base(context) { }

        public async Task<IEnumerable<Estoque>> GetAbaixoMinimoAsync(int unidadeId)
        {
            return await _dbSet.AsNoTracking()
                .Include(e => e.Produtos)
                .Where(e => e.UnidadeId.Equals(unidadeId) && e.Quantidade <= e.MinimoAlerta)
                .ToListAsync();
        }

        public async Task<IEnumerable<Estoque>> GetByProdutosEUnidadeAsync(IEnumerable<int> produtoIds, int unidadeId)
        {
            return await _dbSet
                .Where(e => e.UnidadeId == unidadeId && produtoIds.Contains(e.ProdutoId))
                .ToListAsync();
        }

        public async Task<Estoque?> GetByProdutoUnidadeAsync(int produtoId, int unidadeId)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.ProdutoId.Equals(produtoId) && e.UnidadeId.Equals(unidadeId));
        }

        public async Task<IEnumerable<Estoque>> GetByUnidadeAsync(int unidadeId)
        {
            return await _dbSet.AsNoTracking()
                .Include(e => e.Produtos)
                .Where(e => e.UnidadeId.Equals(unidadeId))
                .OrderBy(e => e.Produtos.Nome)
                .ToListAsync();
        }
    }
}
