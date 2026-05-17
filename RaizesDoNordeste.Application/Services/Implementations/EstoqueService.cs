using RaizesDoNordeste.Application.Common;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;
using RaizesDoNordeste.Application.Services.Interfaces;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Repositories;

namespace RaizesDoNordeste.Application.Services.Implementations
{
    public class EstoqueService : IEstoqueService
    {
        private readonly IEstoqueRepository _estoqueRepository;

        public EstoqueService(IEstoqueRepository estoqueRepository)
        {
            _estoqueRepository = estoqueRepository;
        }

        public async Task<Result<EstoqueResponse>> EntradaAsync(MovimentarEstoqueRequest request)
        {
            var estoque = await _estoqueRepository.GetByProdutoUnidadeAsync(request.ProdutoId, request.UnidadeId);
            if (estoque is null)
            {
                return Result<EstoqueResponse>.Failure("Estoque não encontrado para este produto/unidade.");
            }

            estoque.Quantidade += request.Quantidade;
            estoque.AtualizadoEm = DateTime.UtcNow.AddHours(-3);

            _estoqueRepository.Update(estoque);
            await _estoqueRepository.SaveChangesAsync();

            return Result<EstoqueResponse>.Success(ToResponse(estoque));
        }

        public async Task<Result<IEnumerable<EstoqueResponse>>> ListarAbaixoMinimoAsync(int unidadeId)
        {
            var estoque = await _estoqueRepository.GetAbaixoMinimoAsync(unidadeId);
            return Result<IEnumerable<EstoqueResponse>>.Success(estoque.Select(ToResponse));
        }

        public async Task<Result<IEnumerable<EstoqueResponse>>> ListarPorUnidadeAsync(int unidadeId)
        {
            var estoque = await _estoqueRepository.GetByUnidadeAsync(unidadeId);
            return Result<IEnumerable<EstoqueResponse>>.Success(estoque.Select(ToResponse));
        }

        public async Task<Result<EstoqueResponse>> ObterSaldoAsync(int produtoId, int unidadeId)
        {
            var estoque = await _estoqueRepository.GetByProdutoUnidadeAsync(produtoId, unidadeId);
            if (estoque is null)
            {
                return Result<EstoqueResponse>.Failure("Estoque não encontrado para este produto/unidade.");
            }

            return Result<EstoqueResponse>.Success(ToResponse(estoque));
        }

        public async Task<Result<EstoqueResponse>> SaidaAsync(MovimentarEstoqueRequest request)
        {
            var estoque = await _estoqueRepository.GetByProdutoUnidadeAsync(request.ProdutoId, request.UnidadeId);
            if (estoque is null)
            {
                return Result<EstoqueResponse>.Failure("Estoque não encontrado para este produto/unidade.");
            }

            if (!estoque.PossuiEstoque(request.Quantidade))
            {
                return Result<EstoqueResponse>.Failure($"Estoque insuficiente. Disponível: {estoque.Quantidade}");
            }

            estoque.Quantidade -= request.Quantidade;
            estoque.AtualizadoEm = DateTime.UtcNow.AddHours(-3);

            _estoqueRepository.Update(estoque);
            await _estoqueRepository.SaveChangesAsync();

            return Result<EstoqueResponse>.Success(ToResponse(estoque));

        }
        private static EstoqueResponse ToResponse(Domain.Entities.Estoque e)
        {
            return new EstoqueResponse(
               e.ProdutoId,
               e.Produtos?.Nome ?? string.Empty,
               e.UnidadeId,
               e.Quantidade,
               e.MinimoAlerta,
               e.Quantidade <= e.MinimoAlerta
            );
        }
    }
}
