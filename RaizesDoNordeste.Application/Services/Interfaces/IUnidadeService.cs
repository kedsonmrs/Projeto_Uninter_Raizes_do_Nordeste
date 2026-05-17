using RaizesDoNordeste.Application.Common;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;

namespace RaizesDoNordeste.Application.Services.Interfaces
{
    public interface IUnidadeService
    {
        Task<Result<IEnumerable<UnidadeResponse>>> ListarAtivasAsync();
        Task<Result<UnidadeResponse>> ObterPorIdAsync(int id);
        Task<Result<UnidadeResponse>> CriarAsync(CriarUnidadeRequest request);
        Task<Result<UnidadeResponse>> AtualizarAsync(int id, CriarUnidadeRequest request);
        Task<Result<bool>> DesativarAsync(int id);
    }
}
