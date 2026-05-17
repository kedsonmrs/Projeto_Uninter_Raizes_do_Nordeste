using RaizesDoNordeste.Application.Common;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;

namespace RaizesDoNordeste.Application.Services.Interfaces
{
    public interface IPagamentoService
    {
        Task<Result<PagamentoResponse>> ProcessarAsync(int pedidoId, ProcessarPagamentoRequest request);
        Task<Result<PagamentoResponse>> ObterPorPedidoAsync(int pedidoId);
    }
}
