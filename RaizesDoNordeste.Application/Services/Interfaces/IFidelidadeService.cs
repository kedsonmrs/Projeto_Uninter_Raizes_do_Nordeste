using RaizesDoNordeste.Application.Common;
using RaizesDoNordeste.Application.ResponseViewModel;

namespace RaizesDoNordeste.Application.Services.Interfaces
{
    public interface IFidelidadeService
    {
        Task<Result<FidelidadeResponse>> ObterSaldoAsync(Guid usuarioId);
        Task<Result<IEnumerable<TransacaoFidelidadeResponse>>> ListarTransacoesAsync(Guid usuarioId);
        Task AcumularPontosAsync(Guid usuarioId, int pedidoId, decimal totalPedido);
        Task<Result<FidelidadeResponse>> ResgatarPontosAsync(Guid usuarioId, int pontosResgatar);
    }
}
