using Core.Application;

namespace Application.Usuario.DomainEvents
{
    internal sealed class UsuarioVisitanteCreado : DomainEvent
    {
        public int UsuarioVisitanteId { get; set; }
        public string Nombre { get; set; }   // podemos extraer del VO
        public string Apellido { get; set; } // idem
        public string Email { get; set; }    // idem
        public string Telefono { get; set; }    // idem

    }
}
