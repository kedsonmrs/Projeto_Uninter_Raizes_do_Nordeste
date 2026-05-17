using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Domain.Repositories
{
    public interface IProdutoRepository : IBaseRepository<Produto, int>
    {
        Task<IEnumerable<Produto>> GetByUnidadeAsync(int unidadeId, bool somenteDisponiveis = true);
    }
}
