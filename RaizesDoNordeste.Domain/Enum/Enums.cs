namespace RaizesDoNordeste.Domain.Enum
{
    public enum RoleUsuario
    {
        Admin = 0,
        Cliente = 1,
        Cozinha = 2,
        Atendente = 3,
        Gerente = 4
    }

    public enum StatusPedido
    {
        AguardandoPagamento = 0,
        Confirmado = 1,
        EmPreparo = 2,
        Pronto = 3,
        Entregue = 4,
        Cancelado = 5
    }

    public enum CanalPedido
    {
        App =  0,
        Totem = 1,
        Balcao = 2,
        Pickup = 3,
        Web = 4
    }

    public enum StatusPagamento
    {
        Pendente = 0,
        Aprovado = 1,
        Recusado = 2
    }

    public enum MetodoPagamento
    {
        Mock = 0,
        Pix = 1,
        Cartao = 2  
    }

    public enum TipoTransacaoFidelidade
    {
        Acumulo = 0,
        Resgate = 1
    }

}
