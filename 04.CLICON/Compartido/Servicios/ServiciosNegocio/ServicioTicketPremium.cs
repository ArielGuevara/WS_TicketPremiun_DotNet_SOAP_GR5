using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Compartido.Modelos.Negocio;
using Compartido.Servicios.Comunicacion;
using Compartido.Servicios.Parseadores;

namespace Compartido.Servicios.Negocio
{
    /// <summary>
    /// Cliente de negocio que consume el servicio WCF TicketPremiumService vía SOAP.
    /// Provee las operaciones de autenticación: iniciar sesión, cerrar sesión y registro.
    /// Esta clase es la que deben usar los proyectos Web (Blazor) y Móvil (MAUI).
    /// </summary>
    public class ServicioTicketPremium
    {
        private readonly string _urlServicio;
        private const string NombreInterfaz = "ITicketPremiumService";

        /// <summary>
        /// Crea una instancia del cliente apuntando al endpoint del servicio WCF.
        /// </summary>
        /// <param name="urlServicio">URL del servicio .svc (ej: http://localhost/TicketPremiumService.svc)</param>
        public ServicioTicketPremium(string urlServicio)
        {
            _urlServicio = urlServicio ?? throw new ArgumentNullException(nameof(urlServicio));
        }

        // ──────────────────────────────────────────────
        // Método 1: Iniciar sesión
        // ──────────────────────────────────────────────

        /// <summary>
        /// Autentica al usuario con sus credenciales (correo y contraseña).
        /// Invoca la operación IniciarSesion del WCF.
        /// </summary>
        /// <param name="correo">Correo electrónico del usuario</param>
        /// <param name="password">Contraseña en texto plano (se valida contra el hash en el servidor)</param>
        /// <returns>UsuarioDTO con los datos del usuario y token de sesión, o null si las credenciales son inválidas</returns>
        public async Task<UsuarioDTO?> IniciarSesion(string correo, string password)
        {
            var peticion = new PeticionSOAP<UsuarioDTO?>();

            var parametros = new Dictionary<string, string>
            {
                { "correo", correo },
                { "password", password }
            };

            return await peticion.EnviarPeticion(
                _urlServicio,
                "IniciarSesion",
                parametros,
                parseadorRespuesta: TicketPremiumParsers.ParsearUsuario,
                nombreInterfaz: NombreInterfaz
            );
        }

        // ──────────────────────────────────────────────
        // Método 2: Cerrar sesión
        // ──────────────────────────────────────────────

        /// <summary>
        /// Cierra la sesión del usuario invalidando su token.
        /// Invoca la operación CerrarSesion del WCF.
        /// </summary>
        /// <param name="token">Token de sesión obtenido al iniciar sesión</param>
        /// <returns>true si la sesión se cerró correctamente</returns>
        public async Task<bool> CerrarSesion(string token)
        {
            var peticion = new PeticionSOAP<bool>();

            var parametros = new Dictionary<string, string>
            {
                { "token", token }
            };

            return await peticion.EnviarPeticion(
                _urlServicio,
                "CerrarSesion",
                parametros,
                parseadorRespuesta: TicketPremiumParsers.ParsearCerrarSesion,
                nombreInterfaz: NombreInterfaz
            );
        }

        // ──────────────────────────────────────────────
        // Método 3: Registrar usuario
        // ──────────────────────────────────────────────

        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// Invoca la operación RegistrarUsuario del WCF.
        /// </summary>
        /// <param name="nombres">Nombres completos del usuario</param>
        /// <param name="correo">Correo electrónico (debe ser único)</param>
        /// <param name="password">Contraseña en texto plano (se encripta con BCrypt en el servidor)</param>
        /// <returns>true si el registro fue exitoso, false si el correo ya está registrado</returns>
        public async Task<bool> RegistrarUsuario(string nombres, string correo, string password)
        {
            var peticion = new PeticionSOAP<bool>();

            var parametros = new Dictionary<string, string>
            {
                { "nombres", nombres },
                { "correo", correo },
                { "password", password }
            };

            return await peticion.EnviarPeticion(
                _urlServicio,
                "RegistrarUsuario",
                parametros,
                parseadorRespuesta: TicketPremiumParsers.ParsearRegistrarUsuario,
                nombreInterfaz: NombreInterfaz
            );
        }

        // ──────────────────────────────────────────────
        // Método 4: Comprar boletos (nuevo, genera factura)
        // ──────────────────────────────────────────────

        /// <summary>
        /// Compra boletos a través del TicketPremiumService.
        /// Internamente descuenta disponibilidad en Federación y genera una factura.
        /// </summary>
        public async Task<FacturaDTO> ComprarBoletos(int idUsuario, int codigoPartido, string codigoLocalidad, int cantidadBoletos, decimal precioUnitario)
        {
            var peticion = new PeticionSOAP<FacturaDTO>();

            var parametros = new Dictionary<string, string>
            {
                { "idUsuario", idUsuario.ToString() },
                { "codigoPartido", codigoPartido.ToString() },
                { "codigoLocalidad", codigoLocalidad },
                { "cantidadBoletos", cantidadBoletos.ToString() },
                { "precioUnitario", precioUnitario.ToString(System.Globalization.CultureInfo.InvariantCulture) }
            };

            return await peticion.EnviarPeticion(
                _urlServicio,
                "ComprarBoletos",
                parametros,
                parseadorRespuesta: TicketPremiumParsers.ParsearFactura,
                nombreInterfaz: NombreInterfaz
            );
        }

        // ──────────────────────────────────────────────
        // Método 5: Obtener resumen de ventas
        // ──────────────────────────────────────────────

        public async Task<ReporteResumenVentasDTO> ObtenerResumenVentas(int codigoPartido)
        {
            var peticion = new PeticionSOAP<ReporteResumenVentasDTO>();

            var parametros = new Dictionary<string, string>
            {
                { "codigoPartido", codigoPartido.ToString() }
            };

            return await peticion.EnviarPeticion(
                _urlServicio,
                "ObtenerResumenVentas",
                parametros,
                parseadorRespuesta: TicketPremiumParsers.ParsearResumenVentas,
                nombreInterfaz: NombreInterfaz
            );
        }
    }
}
