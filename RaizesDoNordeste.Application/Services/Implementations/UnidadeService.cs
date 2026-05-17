using RaizesDoNordeste.Application.Common;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;
using RaizesDoNordeste.Application.Services.Interfaces;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Repositories;

namespace RaizesDoNordeste.Application.Services.Implementations
{
    public class UnidadeService : IUnidadeService
    {
        private readonly IUnidadeRepository _unidadeRepository;

        public UnidadeService(IUnidadeRepository unidadeRepository)
        {
            _unidadeRepository = unidadeRepository;
        }

        public async Task<Result<UnidadeResponse>> AtualizarAsync(int id, CriarUnidadeRequest request)
        {
            var unidade = await _unidadeRepository.GetByIdAsync(id);
            if (unidade is null)
            {
                return Result<UnidadeResponse>.Failure("Unidade não encontrada");
            }

            unidade.Nome = request.Nome.Trim();
            unidade.Endereco = request.Endereco.Trim();
            unidade.Telefone = request.Telefone?.Trim();

            _unidadeRepository.Update(unidade);
            await _unidadeRepository.SaveChangesAsync();

            return Result<UnidadeResponse>.Success(ToResponse(unidade));

        }

        public async Task<Result<UnidadeResponse>> CriarAsync(CriarUnidadeRequest request)
        {
            var unidade = new Unidade
            {
                Nome = request.Nome.Trim(),
                Endereco = request.Endereco.Trim(),
                Telefone = request.Telefone?.Trim()
            };

            _unidadeRepository.Add(unidade);
            await _unidadeRepository.SaveChangesAsync();

            return Result<UnidadeResponse>.Success(ToResponse(unidade));
        }

        public async Task<Result<bool>> DesativarAsync(int id)
        {
            var unidade = await _unidadeRepository.GetByIdAsync(id);
            if (unidade is null)
            {
                return Result<bool>.Failure("Unidade não encontrada");
            }

            unidade.Ativa = false;
            _unidadeRepository.Update(unidade);
            await _unidadeRepository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<UnidadeResponse>>> ListarAtivasAsync()
        {
            var unidades = await _unidadeRepository.GetAllAtivasAsync();

            var unidadeResponse = unidades.Select(u => ToResponse(u));

            return Result<IEnumerable<UnidadeResponse>>.Success(unidadeResponse);
        }

        public async Task<Result<UnidadeResponse>> ObterPorIdAsync(int id)
        {
            var unidade = await _unidadeRepository.GetByIdAsync(id);
            if (unidade is null)
            {
                return Result<UnidadeResponse>.Failure("Unidade não encontrada");
            }

            return Result<UnidadeResponse>.Success(ToResponse(unidade));
        }


        private static UnidadeResponse ToResponse(Unidade u)
            => new(u.Id, u.Nome, u.Endereco, u.Telefone, u.Ativa);
    }
}
