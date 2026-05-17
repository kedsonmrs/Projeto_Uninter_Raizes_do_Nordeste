using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Domain.Repositories
{
    public interface IFedilidadeRepository : IBaseRepository<PontoFidelidade, int>
    {
        Task<PontoFidelidade?> GetByUsuarioIdAsync(Guid usuarioId);
        Task<IEnumerable<TransacaoFidelidade>> GetTransacoesAsync(Guid usuarioId);
        Task AddTransacaoAsync(TransacaoFidelidade transacao);

    }
}
