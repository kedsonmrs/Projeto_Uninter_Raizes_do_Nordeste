using RaizesDoNordeste.Application.Common;
using RaizesDoNordeste.Application.ResponseViewModel;
using RaizesDoNordeste.Application.Services.Interfaces;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Enum;
using RaizesDoNordeste.Domain.Repositories;

namespace RaizesDoNordeste.Application.Services.Implementations
{
    public class FidelidadeService : IFidelidadeService
    {
        private readonly IFedilidadeRepository _fidelidadeRepository;
        private const decimal PorcentagemPontos = 1m;

        public FidelidadeService(IFedilidadeRepository fidelidadeRepository)
        {
            _fidelidadeRepository = fidelidadeRepository;
        }

        public async Task AcumularPontosAsync(Guid usuarioId, int pedidoId, decimal totalPedido)
        {
            var fidelidade = await _fidelidadeRepository.GetByUsuarioIdAsync(usuarioId);
            if (fidelidade is null)
            {
                Result<FidelidadeResponse>.Failure("Fidelidade não encontrada.");
            }

            var pontos = (int)(totalPedido * PorcentagemPontos);

            fidelidade?.PontosTotal += pontos;
            fidelidade?.AtualizadoEm = DateTime.UtcNow.AddHours(-3);
            _fidelidadeRepository.Update(fidelidade);

            await _fidelidadeRepository.AddTransacaoAsync(new TransacaoFidelidade
            {
                UsuarioId = usuarioId,
                PedidoId = pedidoId,
                Tipo = TipoTransacaoFidelidade.Acumulo,
                Pontos = pontos,
                Descricao = $"Acúmulo de {pontos} pontos referente ao pedido.",
            });

            await _fidelidadeRepository.SaveChangesAsync();
        }

        public async Task<Result<IEnumerable<TransacaoFidelidadeResponse>>> ListarTransacoesAsync(Guid usuarioId)
        {
            var transacoes = await _fidelidadeRepository.GetTransacoesAsync(usuarioId);
            var transacoesResponse = transacoes.Select(t => new TransacaoFidelidadeResponse(
                t.Id,
                t.Tipo.ToString(),
                t.Pontos,
                t.Descricao,
                t.CriadoEm)
            );

            return Result<IEnumerable<TransacaoFidelidadeResponse>>.Success(transacoesResponse);
        }

        public async Task<Result<FidelidadeResponse>> ObterSaldoAsync(Guid usuarioId)
        {
            var fidelidade = await _fidelidadeRepository.GetByUsuarioIdAsync(usuarioId);
            if (fidelidade is null)
            {
                return Result<FidelidadeResponse>.Failure("Programa de fidelidade não encontrado para este usuário.");
            }

            return Result<FidelidadeResponse>.Success(ToResponse(fidelidade));
        }

        public async Task<Result<FidelidadeResponse>> ResgatarPontosAsync(Guid usuarioId, int pontosResgatar)
        {
            var fidelidade = await _fidelidadeRepository.GetByUsuarioIdAsync(usuarioId);
            if (fidelidade is null)
            {
                return Result<FidelidadeResponse>.Failure("Fidelidade não encontrada.");
            }
  

            if (fidelidade.PontosTotal < pontosResgatar)
            {
                return Result<FidelidadeResponse>.Failure($"Pontos insuficientes. Saldo atual: {fidelidade.PontosTotal}");
            }

            fidelidade.PontosTotal -= pontosResgatar;
            fidelidade.AtualizadoEm = DateTime.UtcNow;
            _fidelidadeRepository.Update(fidelidade);

            await _fidelidadeRepository.AddTransacaoAsync(new TransacaoFidelidade
            {
                UsuarioId = usuarioId,
                Tipo = TipoTransacaoFidelidade.Resgate,
                Pontos = -pontosResgatar,
                Descricao = $"Resgate de {pontosResgatar} pontos."
            });

            await _fidelidadeRepository.SaveChangesAsync();
            return Result<FidelidadeResponse>.Success(ToResponse(fidelidade));
        }

        private static FidelidadeResponse ToResponse(PontoFidelidade f)
        {
            return new(f.UsuarioId, f.PontosTotal, f.AtualizadoEm);
        }
    }
}
