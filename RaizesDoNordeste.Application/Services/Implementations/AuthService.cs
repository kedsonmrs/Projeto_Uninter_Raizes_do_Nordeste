using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RaizesDoNordeste.Application.Common;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;
using RaizesDoNordeste.Application.Services.Interfaces;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enum;
using RaizesDoNordeste.Domain.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RaizesDoNordeste.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IFedilidadeRepository _fidelidadeRepository;
        private readonly IConfiguration _config;

        public AuthService(IUsuarioRepository usuarioRepository, IFedilidadeRepository fidelidadeRepository, IConfiguration config)
        {
            _usuarioRepository = usuarioRepository;
            _fidelidadeRepository = fidelidadeRepository;
            _config = config;
        }

        public async Task<Result<UsuarioResumo>> CadastrasAsync(CadastrarUsuarioRequest request)
        {
            if (await _usuarioRepository.EmailExisteAsync(request.Email))
            {
                return Result<UsuarioResumo>.Failure("E-mail já cadastrado.");
            }

            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = request.Nome.Trim(),
                Email = request.Email.Trim().ToLower(),
                SenhaHash = HashSenha(request.Senha),
                Telefone = request.Telefone?.Trim(),
                Role = RoleUsuario.Cliente,
                ConsentimentoLGPD = true,
                ConsentimentoEm = DateTime.UtcNow,
                CriadoEm = DateTime.UtcNow,
                Ativo = true
            };

            _usuarioRepository.Add(usuario);

            _fidelidadeRepository.Add(new PontoFidelidade
            {
                UsuarioId = usuario.Id,
                PontosTotal = 0,
                AtualizadoEm = DateTime.UtcNow
            });

            await _usuarioRepository.SaveChangesAsync();

            var resumo = new UsuarioResumo(usuario.Id, usuario.Nome, usuario.Email, usuario.Role.ToString());

            return Result<UsuarioResumo>.Success(resumo);
        }

        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);
            if (usuario is null)
            {
                return Result<LoginResponse>.Failure("Credenciais inválidas.");
            }

            if (!VerificaSenha(request.Senha, usuario.SenhaHash))
            {
                return Result<LoginResponse>.Failure("Credenciais inválidas.");
            }

            var token = GerarToken(usuario);

            var login = new LoginResponse(token, "Bearer", 28800, new UsuarioResumo(usuario.Id, usuario.Nome, usuario.Email, usuario.Role.ToString()));
            
            return Result<LoginResponse>.Success(login);

        }

        private string HashSenha(string senha)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return Convert.ToBase64String(bytes);
        }

        private bool VerificaSenha(string senhaPura, string senhaHash)
        {
            return HashSenha(senhaPura) == senhaHash;
        }

        private string GerarToken(Usuario usuario)
        {
            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "Chave não encontrada"));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Role, usuario.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
