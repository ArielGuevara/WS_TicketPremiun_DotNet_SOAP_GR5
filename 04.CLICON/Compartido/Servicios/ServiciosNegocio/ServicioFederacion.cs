using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Compartido.Modelos.Negocio;
using Compartido.Servicios.Comunicacion;
using Compartido.Servicios.Parseadores;

namespace Compartido.Servicios.Negocio
{
    /// <summary>
    /// Cliente de negocio que consume el servicio WCF FederacionService vía SOAP.
    /// Esta clase es la que deben usar los proyectos Web (Blazor) y Móvil (MAUI).
    /// </summary>
    public class ServicioFederacion
    {
        private readonly string _urlServicio;

        /// <summary>
        /// Crea una instancia del cliente apuntando al endpoint del servicio WCF.
        /// </summary>
        /// <param name="urlServicio">URL del servicio .svc</param>
        public ServicioFederacion(string urlServicio)
        {
            _urlServicio = urlServicio ?? throw new ArgumentNullException(nameof(urlServicio));
        }

        // ──────────────────────────────────────────────
        // Método 1: Obtener partidos
        // ──────────────────────────────────────────────

        /// <summary>
        /// Obtiene la lista de partidos cuya fecha es mayor o igual a la fecha actual.
        /// Invoca la operación ObtenerPartidosDisponibles del WCF.
        /// </summary>
        public async Task<List<PartidoDTO>> ObtenerPartidosDisponibles()
        {
            var peticion = new PeticionSOAP<List<PartidoDTO>>();

            return await peticion.EnviarPeticion(
                _urlServicio,
                "ObtenerPartidosDisponibles",
                parametros: null,
                parseadorRespuesta: FederacionParsers.ParsearPartidos
            );
        }

        // ──────────────────────────────────────────────
        // Método 2: Obtener localidades disponibles
        // ──────────────────────────────────────────────

        /// <summary>
        /// Obtiene las localidades de un partido que tienen disponibilidad.
        /// Invoca la operación ObtenerLocalidadesDisponibles del WCF.
        /// </summary>
        /// <param name="codigoPartido">Código del partido seleccionado</param>
        public async Task<List<LocalidadDTO>> ObtenerLocalidadesDisponibles(int codigoPartido)
        {
            var peticion = new PeticionSOAP<List<LocalidadDTO>>();

            var parametros = new Dictionary<string, string>
            {
                { "codigoPartido", codigoPartido.ToString() }
            };

            return await peticion.EnviarPeticion(
                _urlServicio,
                "ObtenerLocalidadesDisponibles",
                parametros,
                parseadorRespuesta: FederacionParsers.ParsearLocalidades
            );
        }

        // ──────────────────────────────────────────────
        // Método 3: Disminuir disponibilidad (comprar)
        // ──────────────────────────────────────────────

        /// <summary>
        /// Decrementa la disponibilidad de boletos en una localidad específica.
        /// Invoca la operación DisminuirDisponibilidad del WCF.
        /// </summary>
        /// <param name="codigoPartido">Código del partido</param>
        /// <param name="codigoLocalidad">Código de la localidad</param>
        /// <param name="boletosComprados">Cantidad de boletos a comprar</param>
        /// <returns>true si la compra fue exitosa, false si no hay suficientes boletos</returns>
        public async Task<bool> ComprarBoletos(int codigoPartido, string codigoLocalidad, int boletosComprados)
        {
            var peticion = new PeticionSOAP<bool>();

            var parametros = new Dictionary<string, string>
            {
                { "codigoPartido", codigoPartido.ToString() },
                { "codigoLocalidad", codigoLocalidad },
                { "boletosComprados", boletosComprados.ToString() }
            };

            return await peticion.EnviarPeticion(
                _urlServicio,
                "DisminuirDisponibilidad",
                parametros,
                parseadorRespuesta: FederacionParsers.ParsearBooleano
            );
        }
    }
}
