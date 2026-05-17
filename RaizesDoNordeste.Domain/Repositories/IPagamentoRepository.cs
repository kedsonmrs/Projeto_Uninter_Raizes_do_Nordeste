using RaizesDoNordeste.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaizesDoNordeste.Domain.Repositories
{
    public interface IPagamentoRepository : IBaseRepository<Pagamento, int>
    {
        Task<Pagamento?> GetByPedidoAsync(int pedidoId);
    }
}
