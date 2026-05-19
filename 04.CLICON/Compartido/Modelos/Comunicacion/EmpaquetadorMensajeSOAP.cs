using System.Collections.Generic;
using System.Text;

namespace Compartido.Modelos.Comunicacion
{
    /// <summary>
    /// Construye sobres SOAP de forma dinámica para invocar operaciones WCF.
    /// </summary>
    public static class EmpaquetadorMensajeSOAP
    {
        private const string SoapNs = "http://schemas.xmlsoap.org/soap/envelope/";
        private const string DefaultTargetNs = "http://tempuri.org/";

        /// <summary>
        /// Genera el XML completo del sobre SOAP para una operación dada.
        /// </summary>
        /// <param name="operacion">Nombre de la operación (ej: "ObtenerPartidosDisponibles")</param>
        /// <param name="parametros">Diccionario de nombre-valor para los parámetros de la operación (puede ser null)</param>
        /// <param name="targetNamespace">Namespace del servicio WCF (por defecto http://tempuri.org/)</param>
        /// <returns>Cadena XML del sobre SOAP</returns>
        public static string Construir(string operacion, IDictionary<string, string>? parametros = null, string targetNamespace = DefaultTargetNs)
        {
            var sb = new StringBuilder();

            sb.Append($@"<?xml version=""1.0"" encoding=""utf-8""?>");
            sb.Append($@"<soap:Envelope xmlns:soap=""{SoapNs}"" xmlns:tem=""{targetNamespace}"">");
            sb.Append(@"<soap:Header/>");
            sb.Append(@"<soap:Body>");
            sb.Append($@"<tem:{operacion}>");

            if (parametros != null)
            {
                foreach (var param in parametros)
                {
                    sb.Append($"<tem:{param.Key}>{param.Value}</tem:{param.Key}>");
                }
            }

            sb.Append($@"</tem:{operacion}>");
            sb.Append(@"</soap:Body>");
            sb.Append(@"</soap:Envelope>");

            return sb.ToString();
        }
    }
}
