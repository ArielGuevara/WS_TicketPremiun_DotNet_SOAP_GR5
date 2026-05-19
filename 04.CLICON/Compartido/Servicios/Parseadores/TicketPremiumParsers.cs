using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Compartido.Modelos.Negocio;

namespace Compartido.Servicios.Parseadores
{
    /// <summary>
    /// Parseadores de respuestas SOAP XML para el servicio WCF TicketPremiumService.
    /// Convierte los elementos XML del Body en DTOs de negocio.
    /// </summary>
    public static class TicketPremiumParsers
    {
        private static readonly XNamespace TempuriNs = "http://tempuri.org/";
        private static readonly XNamespace ContractNs = "http://schemas.datacontract.org/2004/07/Monster.Edu.Ec.TicketPremium.WCF.Modelos";

        // ══════════════════════════════════════════════
        //  Parser: IniciarSesion → UsuarioDTO (o null)
        // ══════════════════════════════════════════════

        /// <summary>
        /// Parsea la respuesta XML de IniciarSesion y devuelve un UsuarioDTO.
        /// Retorna null si las credenciales son incorrectas (el servicio devuelve nil).
        /// </summary>
        public static UsuarioDTO? ParsearUsuario(XElement body)
        {
            var resultado = body.Descendants(TempuriNs + "IniciarSesionResult").FirstOrDefault();

            if (resultado == null || resultado.IsEmpty)
                return null;

            // Verificar si el elemento tiene el atributo xsi:nil="true" (credenciales inválidas)
            XNamespace xsiNs = "http://www.w3.org/2001/XMLSchema-instance";
            var atributoNil = resultado.Attribute(xsiNs + "nil");
            if (atributoNil != null && atributoNil.Value == "true")
                return null;

            return new UsuarioDTO
            {
                IdUsuario = int.Parse(resultado.Element(ContractNs + "IdUsuario")?.Value ?? "0"),
                Nombres = resultado.Element(ContractNs + "Nombres")?.Value ?? string.Empty,
                Correo = resultado.Element(ContractNs + "Correo")?.Value ?? string.Empty,
                TokenSession = resultado.Element(ContractNs + "TokenSession")?.Value ?? string.Empty
            };
        }

        // ══════════════════════════════════════════════
        //  Parser: CerrarSesion → bool
        // ══════════════════════════════════════════════

        /// <summary>
        /// Parsea la respuesta XML de CerrarSesion y devuelve un booleano.
        /// </summary>
        public static bool ParsearCerrarSesion(XElement body)
        {
            var resultado = body.Descendants(TempuriNs + "CerrarSesionResult").FirstOrDefault();

            if (resultado == null)
                return false;

            return bool.Parse(resultado.Value);
        }

        // ══════════════════════════════════════════════
        //  Parser: RegistrarUsuario → bool
        // ══════════════════════════════════════════════

        /// <summary>
        /// Parsea la respuesta XML de RegistrarUsuario y devuelve un booleano.
        /// true = registro exitoso, false = el correo ya existe.
        /// </summary>
        public static bool ParsearRegistrarUsuario(XElement body)
        {
            var resultado = body.Descendants(TempuriNs + "RegistrarUsuarioResult").FirstOrDefault();

            if (resultado == null)
                return false;

            return bool.Parse(resultado.Value);
        }

        // ══════════════════════════════════════════════
        //  Parser: ComprarBoletos → FacturaDTO
        // ══════════════════════════════════════════════

        // ══════════════════════════════════════════════
        //  Parser: ObtenerResumenVentas → ReporteResumenVentasDTO
        // ══════════════════════════════════════════════

        public static ReporteResumenVentasDTO ParsearResumenVentas(XElement body)
        {
            var resultado = body.Descendants(TempuriNs + "ObtenerResumenVentasResult").FirstOrDefault();
            if (resultado == null)
                return new ReporteResumenVentasDTO { Partido = "Error al obtener reporte" };

            var reporte = new ReporteResumenVentasDTO
            {
                Partido = resultado.Element(ContractNs + "Partido")?.Value ?? string.Empty,
                Fecha = resultado.Element(ContractNs + "Fecha")?.Value ?? string.Empty,
                Detalles = []
            };

            var detalles = resultado.Element(ContractNs + "Detalles");
            if (detalles != null)
            {
                foreach (var d in detalles.Elements(ContractNs + "DetalleReporteDTO"))
                {
                    reporte.Detalles.Add(new DetalleReporteDTO
                    {
                        Localidad = d.Element(ContractNs + "Localidad")?.Value ?? string.Empty,
                        Vendidos = int.Parse(d.Element(ContractNs + "Vendidos")?.Value ?? "0"),
                        TotalRecaudado = decimal.Parse(d.Element(ContractNs + "TotalRecaudado")?.Value ?? "0",
                                                        System.Globalization.CultureInfo.InvariantCulture)
                    });
                }
            }

            return reporte;
        }

        /// <summary>
        /// Parsea la respuesta XML de ComprarBoletos y devuelve un FacturaDTO.
        /// </summary>
        public static FacturaDTO ParsearFactura(XElement body)
        {
            var resultado = body.Descendants(TempuriNs + "ComprarBoletosResult").FirstOrDefault();
            if (resultado == null)
                return new FacturaDTO { Mensaje = "Error: no se recibió respuesta del servidor." };

            return new FacturaDTO
            {
                IdFactura = int.Parse(resultado.Element(ContractNs + "IdFactura")?.Value ?? "0"),
                FechaEmision = DateTime.Parse(resultado.Element(ContractNs + "FechaEmision")?.Value ?? DateTime.MinValue.ToString("O"),
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.RoundtripKind),
                Subtotal = decimal.Parse(resultado.Element(ContractNs + "Subtotal")?.Value ?? "0", CultureInfo.InvariantCulture),
                Iva = decimal.Parse(resultado.Element(ContractNs + "Iva")?.Value ?? "0", CultureInfo.InvariantCulture),
                TotalFinal = decimal.Parse(resultado.Element(ContractNs + "TotalFinal")?.Value ?? "0", CultureInfo.InvariantCulture),
                Mensaje = resultado.Element(ContractNs + "Mensaje")?.Value ?? string.Empty
            };
        }
    }
}
