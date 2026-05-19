namespace Compartido.Modelos.Negocio
{
    /// <summary>
    /// DTO que representa los datos de un usuario autenticado.
    /// Espejo del DataContract del servidor WCF TicketPremiumService.
    /// </summary>
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string TokenSession { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"[{IdUsuario}] {Nombres} — {Correo}";
        }
    }
}
