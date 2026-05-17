using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Domain.Repositories
{
    public interface IUnidadeRepository : IBaseRepository<Unidade, int>
    {
        Task<IEnumerable<Unidade>> GetAllAtivasAsync();
    }
}
