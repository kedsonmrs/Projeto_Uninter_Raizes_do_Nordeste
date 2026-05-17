using RaizesDoNordeste.Domain.Enum;

namespace RaizesDoNordeste.Domain.Entities
{
    public class Usuario : BaseEntity<Guid>
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public RoleUsuario Role { get; set; }
        public bool ConsentimentoLGPD { get; set; }
        public DateTime ConsentimentoEm { get; set; } = DateTime.UtcNow;
        public bool Ativo { get; set; } = true;
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public DateTime? AtualizadoEm { get; set; }

        public ICollection<Pedido> Pedidos { get; set; } = [];
        public PontoFidelidade? PontosFidelidade { get; set; }

    }
}
