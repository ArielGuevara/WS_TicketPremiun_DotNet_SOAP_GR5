using System;

namespace Compartido.Modelos.Negocio
{
    /// <summary>
    /// DTO que representa un partido de fútbol disponible para la venta de boletos.
    /// Espejo del DataContract del servidor WCF.
    /// </summary>
    public class PartidoDTO
    {
        public int Codigo { get; set; }
        public string EquipoLocal { get; set; } = string.Empty;
        public string EquipoVisita { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Lugar { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{EquipoLocal} vs {EquipoVisita} — {Fecha:dd/MM/yyyy HH:mm} — {Lugar}";
        }
    }
}
