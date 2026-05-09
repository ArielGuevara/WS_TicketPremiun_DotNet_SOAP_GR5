using Monster.Edu.Ec.Federacion.WCF.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Monster.Edu.Ec.Federacion.WCF
{
    [ServiceContract]
    public interface IFederacionService
    {
        [OperationContract]
        List<PartidoDTO> ObtenerPartidosDisponibles();

        [OperationContract]
        List<LocalidadDTO> ObtenerLocalidadesDisponibles(int codigoPartido);

        [OperationContract]
        bool DisminuirDisponibilidad(int codigoPartido, string codigoLocalidad, int boletosComprados);
    }
}