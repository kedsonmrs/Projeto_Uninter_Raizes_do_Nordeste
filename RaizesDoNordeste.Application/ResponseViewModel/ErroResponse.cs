namespace RaizesDoNordeste.Application.ResponseViewModel
{
    public record ErroResponse(
    string Error,
    string Message,
    string Path,
    DateTime Timestamp,
    List<DetalheErro>? Details = null);
}
