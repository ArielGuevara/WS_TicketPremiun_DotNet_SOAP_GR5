using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Monster.Edu.Ec.TicketPremium.WCF.Modelos
{
    [DataContract]
    public class ReporteResumenVentasDTO
    {
        [DataMember]
        public string Partido { get; set; }
        [DataMember]
        public string Fecha { get; set; }
        [DataMember]
        public List<DetalleReporteDTO> Detalles { get; set; }
    }

    [DataContract]
    public class DetalleReporteDTO
    {
        [DataMember]
        public string Localidad { get; set; }
        [DataMember]
        public int Vendidos { get; set; }
        [DataMember]
        public decimal TotalRecaudado { get; set; }
    }
}