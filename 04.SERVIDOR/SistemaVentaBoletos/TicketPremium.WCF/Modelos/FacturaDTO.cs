using System;
using System.Runtime.Serialization;

namespace Monster.Edu.Ec.TicketPremium.WCF.Modelos
{
    [DataContract]
    public class FacturaDTO
    {
        [DataMember]
        public int IdFactura { get; set; }
        [DataMember]
        public DateTime FechaEmision { get; set; }
        [DataMember]
        public decimal Subtotal { get; set; }
        [DataMember]
        public decimal Iva { get; set; }
        [DataMember]
        public decimal TotalFinal { get; set; }
        [DataMember]
        public string Mensaje { get; set; }
    }
}