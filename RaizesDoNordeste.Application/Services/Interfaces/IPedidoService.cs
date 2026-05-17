using RaizesDoNordeste.Application.Common;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;
using RaizesDoNordeste.Domain.Enum;

namespace RaizesDoNordeste.Application.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<Result<PedidoResponse>> CriarAsync(Guid usuarioId, CriarPedidoRequest request);
        Task<Result<IEnumerable<PedidoResponse>>> ListarAsync(
          CanalPedido? canalPedido,
          StatusPedido? status,
          int? unidadeId,
          int pagina,
          int limite
        );
        Task<Result<PedidoResponse>> ObterPorIdAsync(int id);
        Task<Result<IEnumerable<PedidoResponse>>> ListarPorUsuarioAsync(Guid usuarioId);
        Task<Result<PedidoResponse>> AtualizarStatusAsync(int id, StatusPedido novoStatus);
        Task<Result<PedidoResponse>> CancelarAsync(int id);
    }
}
