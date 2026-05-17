using RaizesDoNordeste.Application.Common;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;

namespace RaizesDoNordeste.Application.Services.Interfaces
{
    public interface IEstoqueService
    {
        Task<Result<IEnumerable<EstoqueResponse>>> ListarPorUnidadeAsync(int unidadeId);
        Task<Result<EstoqueResponse>> ObterSaldoAsync(int produtoId, int unidadeId);
        Task<Result<EstoqueResponse>> EntradaAsync(MovimentarEstoqueRequest request);
        Task<Result<EstoqueResponse>> SaidaAsync(MovimentarEstoqueRequest request);
        Task<Result<IEnumerable<EstoqueResponse>>> ListarAbaixoMinimoAsync(int unidadeId);
    }
}
