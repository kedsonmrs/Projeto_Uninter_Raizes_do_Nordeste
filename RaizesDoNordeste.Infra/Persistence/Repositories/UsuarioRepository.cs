using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario, Guid>, IUsuarioRepository
    {
        public UsuarioRepository(RaizesDoNordesteDbContext context) : base(context)
        {
            
        }

        public async Task<bool> EmailExisteAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email.Equals(email));
        }
        

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Ativo);
        }
    }
}
