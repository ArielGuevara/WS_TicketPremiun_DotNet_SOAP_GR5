using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Compartido.Modelos.Negocio;

namespace Compartido.Servicios.Parseadores
{
    /// <summary>
    /// Parseadores de respuestas SOAP XML para el servicio WCF de Federación.
    /// Convierte los elementos XML del Body en DTOs de negocio.
    /// </summary>
    public static class FederacionParsers
    {
        private static readonly XNamespace TempuriNs = "http://tempuri.org/";
        private static readonly XNamespace ArraysNs = "http://schemas.datacontract.org/2004/07/Monster.Edu.Ec.Federacion.WCF.Models";

        // ══════════════════════════════════════════════
        //  Parsers de respuesta XML → DTOs
        // ══════════════════════════════════════════════

        /// <summary>
        /// Parsea la respuesta XML de ObtenerPartidosDisponibles y devuelve una lista de PartidoDTO.
        /// </summary>
        public static List<PartidoDTO> ParsearPartidos(XElement body)
        {
            var resultado = body.Descendants(TempuriNs + "ObtenerPartidosDisponiblesResult").FirstOrDefault();

            if (resultado == null)
                return new List<PartidoDTO>();

            return [.. resultado.Elements(ArraysNs + "PartidoDTO")
                .Select(p => new PartidoDTO
                {
                    Codigo = int.Parse(p.Element(ArraysNs + "Codigo")?.Value ?? "0"),
                    EquipoLocal = p.Element(ArraysNs + "EquipoLocal")?.Value ?? string.Empty,
                    EquipoVisita = p.Element(ArraysNs + "EquipoVisita")?.Value ?? string.Empty,
                    Fecha = DateTime.Parse(p.Element(ArraysNs + "Fecha")?.Value ?? DateTime.MinValue.ToString("O"),
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.RoundtripKind),
                    Lugar = p.Element(ArraysNs + "Lugar")?.Value ?? string.Empty
                })];
        }

        /// <summary>
        /// Parsea la respuesta XML de ObtenerLocalidadesDisponibles y devuelve una lista de LocalidadDTO.
        /// </summary>
        public static List<LocalidadDTO> ParsearLocalidades(XElement body)
        {
            var resultado = body.Descendants(TempuriNs + "ObtenerLocalidadesDisponiblesResult").FirstOrDefault();

            if (resultado == null)
                return new List<LocalidadDTO>();

            return [.. resultado.Elements(ArraysNs + "LocalidadDTO")
                .Select(l => new LocalidadDTO
                {
                    CodigoLocalidad = l.Element(ArraysNs + "CodigoLocalidad")?.Value ?? string.Empty,
                    Disponibilidad = int.Parse(l.Element(ArraysNs + "Disponibilidad")?.Value ?? "0"),
                    Precio = decimal.Parse(l.Element(ArraysNs + "Precio")?.Value ?? "0",
                                           CultureInfo.InvariantCulture)
                })];
        }

        /// <summary>
        /// Parsea la respuesta XML de DisminuirDisponibilidad y devuelve un booleano.
        /// </summary>
        public static bool ParsearBooleano(XElement body)
        {
            var resultado = body.Descendants(TempuriNs + "DisminuirDisponibilidadResult").FirstOrDefault();

            if (resultado == null)
                return false;

            return bool.Parse(resultado.Value);
        }
    }
}
