using System.Runtime.Serialization;

namespace Monster.Edu.Ec.TicketPremium.WCF.Modelos
{
    [DataContract]
    public class UsuarioDTO
    {
        [DataMember]
        public int IdUsuario { get; set; }
        [DataMember]
        public string Nombres { get; set; }
        [DataMember]
        public string Correo { get; set; }
        [DataMember]
        public string TokenSession { get; set; }
    }
}