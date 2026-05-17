namespace RaizesDoNordeste.Application.ResponseViewModel
{
    public record TransacaoFidelidadeResponse(
    int Id,
    string Tipo,
    int Pontos,
    string? Descricao,
    DateTime CriadoEm);
}
