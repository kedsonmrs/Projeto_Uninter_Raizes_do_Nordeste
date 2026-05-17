using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories
{
    public class UnidadeRepository : BaseRepository<Unidade, int>, IUnidadeRepository
    {

        public UnidadeRepository(RaizesDoNordesteDbContext context) : base(context) { }

        public async Task<IEnumerable<Unidade>> GetAllAtivasAsync()
        {
            return await _dbSet.AsNoTracking()
                .Where(u => u.Ativa)
                .OrderBy(u => u.Nome)
                .ToListAsync();
        }
    }
}
