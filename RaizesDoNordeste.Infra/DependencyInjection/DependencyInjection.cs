using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RaizesDoNordeste.Domain.Repositories;
using RaizesDoNordeste.Infrastructure.Persistence;
using RaizesDoNordeste.Infrastructure.Persistence.Repositories;

namespace RaizesDoNordeste.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEstoqueRepository, EstoqueRepository>();
            services.AddScoped<IFedilidadeRepository, FidelidadeRepository>();
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IUnidadeRepository, UnidadeRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));

            return services;
        }      

        public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<RaizesDoNordesteDbContext>(opt => opt.UseSqlServer(connectionString));
            return services;
        }
    }
}
