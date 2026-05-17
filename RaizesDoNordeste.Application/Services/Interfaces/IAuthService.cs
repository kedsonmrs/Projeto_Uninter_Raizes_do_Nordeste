using RaizesDoNordeste.Application.Common;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;

namespace RaizesDoNordeste.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UsuarioResumo>> CadastrasAsync(CadastrarUsuarioRequest request);
        Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    }
}
