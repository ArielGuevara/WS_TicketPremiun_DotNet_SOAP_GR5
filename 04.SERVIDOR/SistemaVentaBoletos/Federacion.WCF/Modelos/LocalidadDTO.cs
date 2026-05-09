using System.Runtime.Serialization;

namespace Monster.Edu.Ec.Federacion.WCF.Models
{
    [DataContract]
    public class LocalidadDTO
    {
        [DataMember]
        public string CodigoLocalidad { get; set; }
        [DataMember]
        public int Disponibilidad { get; set; }
        [DataMember]
        public decimal Precio { get; set; }
    }
}