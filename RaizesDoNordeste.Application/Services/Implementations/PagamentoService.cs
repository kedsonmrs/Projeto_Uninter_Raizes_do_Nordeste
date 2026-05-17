using RaizesDoNordeste.Application.Common;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;
using RaizesDoNordeste.Application.Services.Interfaces;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enum;
using RaizesDoNordeste.Domain.Repositories;

namespace RaizesDoNordeste.Application.Services.Implementations
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IPedidoRepository _pedidoRepository;

        public PagamentoService(IPagamentoRepository pagamentoRepository, IPedidoRepository pedidoRepository)
        {
            _pagamentoRepository = pagamentoRepository;
            _pedidoRepository = pedidoRepository;
        }

        public async Task<Result<PagamentoResponse>> ObterPorPedidoAsync(int pedidoId)
        {
            var pagamento = await _pagamentoRepository.GetByIdAsync(pedidoId);
            if (pagamento is null)
            {
                return Result<PagamentoResponse>.Failure("Pagamento não encontrado para este pedido.");
            }

            return Result<PagamentoResponse>.Success(ToResponse(pagamento));
        }

        public async Task<Result<PagamentoResponse>> ProcessarAsync(int pedidoId, ProcessarPagamentoRequest request)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
            if (pedido is null)
            {
                return Result<PagamentoResponse>.Failure("Pedido não encontrado.");
            }

            if (pedido.Status != StatusPedido.AguardandoPagamento)
            {
                return Result<PagamentoResponse>.Failure("Este pedido não está aguardando pagamento.");
            }

            var jaExiste = await _pagamentoRepository.GetByPedidoAsync(pedidoId);
            if (jaExiste is not null)
            {
                return Result<PagamentoResponse>.Failure("Já existe um pagamento registrado para este pedido.");
            }

            var (statusMock, referenciaMock, mensagemMock) = SimularGateway(pedido.Total);

            var pagamento = new Pagamento
            {
                Id = Guid.NewGuid(),
                PedidoId = pedidoId,
                Metodo = request.Metodo,
                Status = statusMock,
                Valor = pedido.Total,
                ReferenciaExterna = referenciaMock,
                MensagemRetorno = mensagemMock,
                ProcessadoEm = DateTime.UtcNow.AddHours(-3)
            };

            _pagamentoRepository.Add(pagamento);

            pedido.Status = statusMock == StatusPagamento.Aprovado
                ? StatusPedido.Confirmado
                : StatusPedido.Cancelado;

            pedido.AtualizadoEm = DateTime.UtcNow;
            _pedidoRepository.Update(pedido);

            await _pagamentoRepository.SaveChangesAsync();

            return Result<PagamentoResponse>.Success(ToResponse(pagamento));
        }

        private static (StatusPagamento status, string referencia, string mensagem) SimularGateway(decimal valor)
        {
            // 80% de chance de aprovação para fins de teste
            var aprovado = Random.Shared.NextDouble() > 0.2;

            return aprovado
                ? (StatusPagamento.Aprovado,
                   $"MOCK-{Guid.NewGuid():N}",
                   "Pagamento aprovado pelo gateway mock.")
                : (StatusPagamento.Recusado,
                   $"MOCK-ERR-{Guid.NewGuid():N}",
                   "Pagamento recusado: saldo insuficiente (simulação).");
        }

        private static PagamentoResponse ToResponse(Pagamento p)
        {
            return new(p.Id, p.PedidoId, p.Metodo.ToString(), p.Status.ToString(),
               p.Valor, p.ReferenciaExterna, p.MensagemRetorno, p.CriadoEm);
        }

    }
}
