using Monster.Edu.Ec.TicketPremium.WCF.Modelos;
using System.ServiceModel;

namespace Monster.Edu.Ec.TicketPremium.WCF
{
    [ServiceContract]
    public interface ITicketPremiumService
    {
        [OperationContract]
        UsuarioDTO IniciarSesion(string correo, string password);

        [OperationContract]
        bool CerrarSesion(string token);

        [OperationContract]
        bool RegistrarUsuario(string nombres, string correo, string password);

        [OperationContract]
        FacturaDTO ComprarBoletos(int idUsuario, int codigoPartido, string codigoLocalidad, int cantidadBoletos, decimal precioUnitario);
    }
}