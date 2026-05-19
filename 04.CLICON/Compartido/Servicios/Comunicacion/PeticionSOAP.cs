using System.Text;
using System.Xml.Linq;
using Compartido.Modelos.Comunicacion;

namespace Compartido.Servicios.Comunicacion
{
    /// <summary>
    /// Clase genérica para enviar peticiones SOAP al servicio WCF de Federación.
    /// TRespuesta es el tipo de retorno esperado tras parsear la respuesta XML.
    /// </summary>
    public class PeticionSOAP<TRespuesta>
    {
        private static readonly HttpClient _clienteHTTP = new();
        private const string TargetNamespace = "http://tempuri.org/";

        /// <summary>
        /// Envía una petición SOAP al endpoint del servicio WCF.
        /// </summary>
        /// <param name="urlServicio">URL completa del servicio</param>
        /// <param name="operacion">Nombre de la operación WCF a invocar</param>
        /// <param name="parametros">Parámetros de la operación (puede ser null para operaciones sin parámetros)</param>
        /// <param name="parseadorRespuesta">Función que recibe el XElement del Body de la respuesta y devuelve TRespuesta</param>
        /// <param name="nombreInterfaz">Nombre de la interfaz del contrato WCF (ej: "IFederacionService", "ITicketPremiumService")</param>
        /// <returns>Resultado parseado de tipo TRespuesta</returns>
        public async Task<TRespuesta> EnviarPeticion(
            string urlServicio,
            string operacion,
            IDictionary<string, string>? parametros,
            Func<XElement, TRespuesta> parseadorRespuesta,
            string nombreInterfaz = "IFederacionService"
        )
        {
            // 1. Construir el sobre SOAP
            string sobreSOAP = EmpaquetadorMensajeSOAP.Construir(operacion, parametros, TargetNamespace);

            // 2. Crear la petición HTTP
            using var peticion = new HttpRequestMessage(HttpMethod.Post, urlServicio);
            peticion.Content = new StringContent(sobreSOAP, Encoding.UTF8, "text/xml");
            peticion.Headers.Add("SOAPAction", $"{TargetNamespace}{nombreInterfaz}/{operacion}");

            // 3. Enviar la petición
            HttpResponseMessage respuesta;
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                respuesta = await _clienteHTTP.SendAsync(peticion, cts.Token);
            }
            catch (TaskCanceledException)
            {
                throw new HttpRequestException($"Tiempo de espera agotado al conectar con {urlServicio}. Verifica que el servicio esté corriendo.");
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"No se pudo conectar con {urlServicio}: {ex.Message}");
            }
            catch (IOException ex)
            {
                throw new HttpRequestException($"Error de red al conectar con {urlServicio}: {ex.Message}. Verifica que el firewall permita la conexión y que uses la IP correcta.");
            }

            if (!respuesta.IsSuccessStatusCode)
            {
                string error = await respuesta.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Error en la petición SOAP a {urlServicio} (operación: {operacion}) [{respuesta.StatusCode}]: {error}");
            }

            // 4. Leer y parsear la respuesta XML
            string respuestaXML = await respuesta.Content.ReadAsStringAsync();
            respuesta.Dispose();
            XDocument documento = XDocument.Parse(respuestaXML);

            // Obtener el Body del sobre SOAP de respuesta
            XNamespace soapNs = "http://schemas.xmlsoap.org/soap/envelope/";
            XElement? cuerpo = documento.Root?.Element(soapNs + "Body");

            return cuerpo == null
                ? throw new InvalidOperationException("La respuesta SOAP no contiene un elemento Body válido.")
                : parseadorRespuesta(cuerpo);
        }
    }
}
