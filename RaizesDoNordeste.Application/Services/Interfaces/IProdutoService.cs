using RaizesDoNordeste.Application.Common;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;

namespace RaizesDoNordeste.Application.Services.Interfaces
{
    public interface IProdutoService
    {
        Task<Result<IEnumerable<ProdutoResponse>>> ListarPorUnidadeAsync(int unidadeId, bool somenteDisponiveis = true);
        Task<Result<ProdutoResponse>> ObterPorIdAsync(int id);
        Task<Result<ProdutoResponse>> CriarAsync(CriarProdutoRequest request);
        Task<Result<ProdutoResponse>> AtualizarAsync(int id, CriarProdutoRequest request);
        Task<Result<bool>> AlterarDisponibilidadeAsync(int id, bool disponivel);
    }
}
