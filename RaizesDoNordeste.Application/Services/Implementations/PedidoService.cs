using RaizesDoNordeste.Application.Common;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;
using RaizesDoNordeste.Application.Services.Interfaces;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enum;
using RaizesDoNordeste.Domain.Repositories;

namespace RaizesDoNordeste.Application.Services.Implementations
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueRepository _estoqueRepository;
        private readonly IFidelidadeService _fidelidadeService;

        public PedidoService(IPedidoRepository pedidoRepository, IProdutoRepository produtoRepository, IEstoqueRepository estoqueRepository, IFidelidadeService fidelidadeService)
        {
            _pedidoRepository = pedidoRepository;
            _produtoRepository = produtoRepository;
            _estoqueRepository = estoqueRepository;
            _fidelidadeService = fidelidadeService;
        }

        public async Task<Result<PedidoResponse>> AtualizarStatusAsync(int id, StatusPedido novoStatus)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido is null)
            {
                return Result<PedidoResponse>.Failure("Produto não encontrado");
            }

            if (pedido.Status == StatusPedido.Cancelado)
            {
                return Result<PedidoResponse>.Failure("Pedido cancelado não pode ser alterado.");
            }

            if (novoStatus < pedido.Status)
            {
                return Result<PedidoResponse>.Failure($"Não é possível regredir o status de '{pedido.Status}' para '{novoStatus}'.");
            }

            pedido.Status = novoStatus;
            pedido.AtualizadoEm = DateTime.UtcNow.AddHours(-3);

            _pedidoRepository.Update(pedido);
            await _pedidoRepository.SaveChangesAsync();

            if (novoStatus == StatusPedido.Entregue)
            {
                await _fidelidadeService.AcumularPontosAsync(pedido.UsuarioId, pedido.Id, pedido.Total);
            }


            return Result<PedidoResponse>.Success(ToResponse(pedido));
        }

        public async Task<Result<PedidoResponse>> CancelarAsync(int id)
        {
            var pedido = await _pedidoRepository.GetByIdComItensAsync(id);
            if (pedido is null)
            {
                return Result<PedidoResponse>.Failure("Produto não encontrado");
            }

            if (pedido.Status == StatusPedido.Entregue)
            {
                return Result<PedidoResponse>.Failure("Pedido já entregue não pode ser cancelado.");
            }

            if (pedido.Status == StatusPedido.Cancelado)
            {
                return Result<PedidoResponse>.Failure("Pedido já está cancelado.");
            }

            var produtosIds = pedido.Itens.Select(ip => ip.ProdutoId).ToList();

            var estoques = await _estoqueRepository.GetByProdutosEUnidadeAsync(produtosIds, pedido.UnidadeId);

            foreach (var item in pedido.Itens)
            {
                var estoque = estoques.FirstOrDefault(e => e.ProdutoId == item.ProdutoId);
                if (estoque is not null)
                {
                    estoque.Quantidade += item.Quantidade;
                    estoque.AtualizadoEm = DateTime.UtcNow.AddHours(-3);
                    _estoqueRepository.Update(estoque);
                }
            }

            pedido.Status = StatusPedido.Cancelado;
            pedido.AtualizadoEm = DateTime.UtcNow.AddHours(-3);

            _pedidoRepository.Update(pedido);
            await _pedidoRepository.SaveChangesAsync();

            return Result<PedidoResponse>.Success(ToResponse(pedido));
        }

        public async Task<Result<PedidoResponse>> CriarAsync(Guid usuarioId, CriarPedidoRequest request)
        {
            if (!request.CanalPedido.HasValue)
            {
                return Result<PedidoResponse>.Failure("O campo canalPedido é obrigatório.");
            }

            var itens = new List<ItemPedido>();
            decimal total = 0;

            foreach (var itemReq in request.Itens)
            {
                var produto = await _produtoRepository.GetByIdAsync(itemReq.ProdutoId);
                if (produto is null)
                {
                    return Result<PedidoResponse>.Failure("Produto não encontrado");
                }

                if (!produto.Disponivel)
                {
                    return Result<PedidoResponse>.Failure($"Produto {produto.Nome} não está disponível");
                }

                var estoque = await _estoqueRepository.GetByProdutoUnidadeAsync(itemReq.ProdutoId, request.UnidadeId);
                if (estoque is null)
                {
                    return Result<PedidoResponse>.Failure($"Estoque encontrado para esse {produto.Nome} nesta unidade.");
                }

                if (!estoque.PossuiEstoque(itemReq.Quantidade))
                {
                    return Result<PedidoResponse>.Failure($"Estoque insuficiente para {produto.Nome}. Disponível: {estoque.Quantidade}.");
                }

                estoque.Quantidade -= itemReq.Quantidade;
                estoque.AtualizadoEm = DateTime.UtcNow.AddHours(-3);
                _estoqueRepository.Update(estoque);

                itens.Add(new ItemPedido
                {
                    ProdutoId = produto.Id,
                    Quantidade = itemReq.Quantidade,
                    PrecoUnitario = produto.Preco
                });

                total += produto.Preco * itemReq.Quantidade;
            }

            var pedido = new Pedido
            {
                UsuarioId = usuarioId,
                UnidadeId = request.UnidadeId,
                CanalPedido = request.CanalPedido ?? 0,
                Total = total,
                Observacao = request.Observacao,
                Itens = itens
            };

            foreach (var item in itens)
            {
                item.PedidoId = pedido.Id;
            }

            _pedidoRepository.Add(pedido);
            await _pedidoRepository.SaveChangesAsync();

            return Result<PedidoResponse>.Success(ToResponse(pedido));
        }

        public async Task<Result<IEnumerable<PedidoResponse>>> ListarAsync(CanalPedido? canalPedido, StatusPedido? status, int? unidadeId, int pagina, int limite)
        {
            var pedido = await _pedidoRepository.GetAllAsync(canalPedido, status, unidadeId, pagina, limite);
            return Result<IEnumerable<PedidoResponse>>.Success(pedido.Select(ToResponse));
        }

        public async Task<Result<IEnumerable<PedidoResponse>>> ListarPorUsuarioAsync(Guid usuarioId)
        {
            var pedidos = await _pedidoRepository.GetByUsuarioAsync(usuarioId);
            return Result<IEnumerable<PedidoResponse>>.Success(pedidos.Select(ToResponse));
        }

        public async Task<Result<PedidoResponse>> ObterPorIdAsync(int id)
        {
            var pedido = await _pedidoRepository.GetByIdComItensAsync(id);
            if (pedido is not null)
            {
                return Result<PedidoResponse>.Success(ToResponse(pedido));
            }
            else
            {
                return Result<PedidoResponse>.Failure("Pedido não encontrado");
            }
        }

        private static PedidoResponse ToResponse(Pedido p)
        {
            return new(p.Id, p.Status.ToString(), p.CanalPedido.ToString(), p.Total, p.CriadoEm, p.Itens.Select(i => new ItemPedidoResponse
            (
                i.ProdutoId,
                i.Produto.Nome ?? string.Empty,
                i.Quantidade,
                i.PrecoUnitario,
                i.SubTotal
            )).ToList());
        }
    }
}
