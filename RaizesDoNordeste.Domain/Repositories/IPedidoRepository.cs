using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enum;

namespace RaizesDoNordeste.Domain.Repositories
{
    public interface IPedidoRepository : IBaseRepository<Pedido, int>
    {
        Task<Pedido?> GetByIdComItensAsync(int id);

        Task<IEnumerable<Pedido>> GetAllAsync(
            CanalPedido? canalPedido = null,
            StatusPedido? status = null,
            int? unidadeId = null,
            int page = 1,
            int limite = 10
        );

        Task<IEnumerable<Pedido>> GetByUsuarioAsync(Guid usuarioId);
    }
}
