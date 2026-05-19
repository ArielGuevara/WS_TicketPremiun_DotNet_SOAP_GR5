using System.Collections.Generic;

namespace Compartido.Modelos.Negocio
{
    public class ReporteResumenVentasDTO
    {
        public string Partido { get; set; } = string.Empty;
        public string Fecha { get; set; } = string.Empty;
        public List<DetalleReporteDTO> Detalles { get; set; } = [];
    }

    public class DetalleReporteDTO
    {
        public string Localidad { get; set; } = string.Empty;
        public int Vendidos { get; set; }
        public decimal TotalRecaudado { get; set; }
    }
}
