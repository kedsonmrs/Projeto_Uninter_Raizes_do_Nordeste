using RaizesDoNordeste.Application.Common;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;
using RaizesDoNordeste.Application.Services.Interfaces;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Repositories;

namespace RaizesDoNordeste.Application.Services.Implementations
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IUnidadeRepository _unidadeRepository;
        private readonly IEstoqueRepository _estoqueRepository;

        public ProdutoService(IProdutoRepository produtoRepository, IUnidadeRepository unidadeRepository, IEstoqueRepository estoqueRepository)
        {
            _produtoRepository = produtoRepository;
            _unidadeRepository = unidadeRepository;
            _estoqueRepository = estoqueRepository;
        }

        public async Task<Result<bool>> AlterarDisponibilidadeAsync(int id, bool disponivel)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto is null)
            {
                return Result<bool>.Failure("Produto não encontrado.");
            }

            produto.Disponivel = disponivel;
            produto.AtualizadoEm = DateTime.UtcNow.AddHours(-3);

            _produtoRepository.Update(produto);
            await _produtoRepository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<ProdutoResponse>> AtualizarAsync(int id, CriarProdutoRequest request)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto is null)
            {
                return Result<ProdutoResponse>.Failure("Produto não encontrado.");
            }

            produto.Nome = request.Nome.Trim();
            produto.Descricao = request.Descricao?.Trim();
            produto.Preco = request.Preco;
            produto.Categoria = request.Categoria.Trim();
            produto.AtualizadoEm = DateTime.UtcNow.AddHours(-3);

            _produtoRepository.Update(produto);
            await _produtoRepository.SaveChangesAsync();

            return Result<ProdutoResponse>.Success(ToResponse(produto));
        }

        public async Task<Result<ProdutoResponse>> CriarAsync(CriarProdutoRequest request)
        {
            var unidadeExiste = await _unidadeRepository.GetByIdAsync(request.UnidadeId);
            if (unidadeExiste is null)
            {
                return Result<ProdutoResponse>.Failure("Unidade não encontrada.");
            }

            var produto = new Produto
            {
                UnidadeId = request.UnidadeId,
                Nome = request.Nome.Trim(),
                Descricao = request.Descricao?.Trim(),
                Preco = request.Preco,
                Categoria = request.Categoria.Trim()
            };

            _produtoRepository.Add(produto);
            await _produtoRepository.SaveChangesAsync();

            _estoqueRepository.Add(new Estoque
            {
                ProdutoId = produto.Id,
                UnidadeId = produto.UnidadeId,
                Quantidade = 0
            });

            await _estoqueRepository.SaveChangesAsync();
            return Result<ProdutoResponse>.Success(ToResponse(produto));
        }

        public async Task<Result<IEnumerable<ProdutoResponse>>> ListarPorUnidadeAsync(int unidadeId, bool somenteDisponiveis = true)
        {
            var produto = await _produtoRepository.GetByUnidadeAsync(unidadeId, somenteDisponiveis);
            return Result<IEnumerable<ProdutoResponse>>.Success(produto.Select(ToResponse));
        }

        public async Task<Result<ProdutoResponse>> ObterPorIdAsync(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto is null)
            {
                return Result<ProdutoResponse>.Failure("Produto não encontado");
            }

            return Result<ProdutoResponse>.Success(ToResponse(produto));
        }

        private static ProdutoResponse ToResponse(Produto p)
        {
            return new ProdutoResponse(p.Id, p.UnidadeId, p.Nome, p.Descricao, p.Preco, p.Categoria, p.Disponivel);
        }
    }
}
