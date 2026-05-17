using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Domain.Repositories
{
    public interface IEstoqueRepository : IBaseRepository<Estoque, int>
    {
        Task<Estoque?> GetByProdutoUnidadeAsync(int produtoId, int unidadeId);
        Task<IEnumerable<Estoque>> GetByUnidadeAsync(int unidadeId);
        Task<IEnumerable<Estoque>> GetAbaixoMinimoAsync(int unidadeId);
        Task<IEnumerable<Estoque>> GetByProdutosEUnidadeAsync(IEnumerable<int> produtoIds, int unidadeId);
    }
}
