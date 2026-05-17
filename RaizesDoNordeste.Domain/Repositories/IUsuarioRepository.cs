using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Domain.Repositories
{
    public interface IUsuarioRepository : IBaseRepository<Usuario, Guid>
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<bool> EmailExisteAsync(string email);
    }
}
