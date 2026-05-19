using Compartido.Modelos.Negocio;
using Compartido.Servicios.Negocio;

static void MostrarEncabezado()
{
    Console.Clear();
    Console.WriteLine("==============================");
    Console.WriteLine("      TICKET PREMIUM MONSTER   ");
    Console.WriteLine("==============================");
    Console.WriteLine();
}

static string LeerTexto(string etiqueta)
{
    Console.Write($"{etiqueta}: ");
    return Console.ReadLine()?.Trim() ?? string.Empty;
}

static int LeerEntero(string etiqueta, int minimo, int maximo)
{
    while (true)
    {
        Console.Write($"{etiqueta}: ");
        var entrada = Console.ReadLine();
        if (int.TryParse(entrada, out var valor) && valor >= minimo && valor <= maximo)
        {
            return valor;
        }

        Console.WriteLine("Valor inválido.");
    }
}

static async Task MostrarPartidosAsync(ServicioFederacion servicioFederacion)
{
    var partidos = await servicioFederacion.ObtenerPartidosDisponibles();
    if (partidos.Count == 0)
    {
        Console.WriteLine("No hay partidos disponibles.");
        return;
    }

    for (var i = 0; i < partidos.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {partidos[i]}");
    }
}

static async Task<List<LocalidadDTO>> MostrarLocalidadesAsync(ServicioFederacion servicioFederacion, int codigoPartido)
{
    var localidades = await servicioFederacion.ObtenerLocalidadesDisponibles(codigoPartido);
    if (localidades.Count == 0)
    {
        Console.WriteLine("No hay localidades disponibles.");
        return localidades;
    }

    for (var i = 0; i < localidades.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {localidades[i]}");
    }

    return localidades;
}

static void EsperarContinuacion()
{
    Console.WriteLine();
    Console.WriteLine("Presiona ENTER para continuar...");
    Console.ReadLine();
}

var urlTicketPremium = "http://localhost:52768/TicketPremiumService.svc";
var urlFederacion = "http://localhost:60235/FederacionService.svc";

var servicioTicketPremium = new ServicioTicketPremium(urlTicketPremium);
var servicioFederacion = new ServicioFederacion(urlFederacion);

UsuarioDTO? usuarioActual = null;

while (true)
{
    MostrarEncabezado();
    Console.WriteLine("1. Iniciar sesión");
    Console.WriteLine("2. Registrar usuario");
    Console.WriteLine("3. Ver partidos disponibles");
    Console.WriteLine("4. Ver localidades disponibles");
    Console.WriteLine("5. Comprar boletos");
    Console.WriteLine("6. Cerrar sesión");
    Console.WriteLine("7. Reporte de ventas");
    Console.WriteLine("0. Salir");
    Console.WriteLine();

    var opcion = LeerEntero("Selecciona una opción", 0, 7);
    Console.WriteLine();

    switch (opcion)
    {
        case 1:
            MostrarEncabezado();
            try
            {
                var correo = LeerTexto("Correo");
                var password = LeerTexto("Contraseña");
                usuarioActual = await servicioTicketPremium.IniciarSesion(correo, password);
                Console.WriteLine(usuarioActual is null
                    ? "Credenciales inválidas."
                    : $"Bienvenido, {usuarioActual.Nombres}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al iniciar sesión: {ex.Message}");
            }
            EsperarContinuacion();
            break;
        case 2:
            MostrarEncabezado();
            try
            {
                var nombres = LeerTexto("Nombres completos");
                var correoRegistro = LeerTexto("Correo");
                var passwordRegistro = LeerTexto("Contraseña");
                var registrado = await servicioTicketPremium.RegistrarUsuario(nombres, correoRegistro, passwordRegistro);
                Console.WriteLine(registrado
                    ? "Usuario registrado correctamente."
                    : "El correo ya está registrado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar: {ex.Message}");
            }
            EsperarContinuacion();
            break;
        case 3:
            MostrarEncabezado();
            try
            {
                await MostrarPartidosAsync(servicioFederacion);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener partidos: {ex.Message}");
            }
            EsperarContinuacion();
            break;
        case 4:
            MostrarEncabezado();
            try
            {
                await MostrarPartidosAsync(servicioFederacion);
                var codigoPartido = LeerEntero("Código del partido", 1, int.MaxValue);
                Console.WriteLine();
                await MostrarLocalidadesAsync(servicioFederacion, codigoPartido);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener localidades: {ex.Message}");
            }
            EsperarContinuacion();
            break;
        case 5:
            MostrarEncabezado();
            if (usuarioActual is null)
            {
                Console.WriteLine("Debes iniciar sesión para comprar.");
                EsperarContinuacion();
                break;
            }

            try
            {
                await MostrarPartidosAsync(servicioFederacion);
                var partidoCompra = LeerEntero("Código del partido", 1, int.MaxValue);
                Console.WriteLine();
                var localidades = await MostrarLocalidadesAsync(servicioFederacion, partidoCompra);
                if (localidades.Count == 0)
                {
                    EsperarContinuacion();
                    break;
                }

                var indiceLocalidad = LeerEntero("Número de localidad", 1, localidades.Count);
                var loc = localidades[indiceLocalidad - 1];
                var cantidad = LeerEntero("Cantidad de boletos", 1, int.MaxValue);
                var factura = await servicioTicketPremium.ComprarBoletos(
                    usuarioActual.IdUsuario, partidoCompra, loc.CodigoLocalidad, cantidad, loc.Precio);
                Console.WriteLine();
                if (factura.IdFactura > 0)
                {
                    Console.WriteLine("══════ FACTURA ══════");
                    Console.WriteLine(factura);
                    Console.WriteLine("═════════════════════");
                }
                else
                {
                    Console.WriteLine($"Error: {factura.Mensaje}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al comprar boletos: {ex.Message}");
            }
            EsperarContinuacion();
            break;
        case 6:
            MostrarEncabezado();
            if (usuarioActual is null)
            {
                Console.WriteLine("No hay sesión activa.");
                EsperarContinuacion();
                break;
            }

            try
            {
                var cerrado = await servicioTicketPremium.CerrarSesion(usuarioActual.TokenSession);
                usuarioActual = null;
                Console.WriteLine(cerrado ? "Sesión cerrada." : "No se pudo cerrar la sesión.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cerrar sesión: {ex.Message}");
            }
            EsperarContinuacion();
            break;
        case 7:
            MostrarEncabezado();
            try
            {
                Console.WriteLine("PARTIDOS DISPONIBLES");
                Console.WriteLine("-------------------");
                await MostrarPartidosAsync(servicioFederacion);
                Console.WriteLine();
                var codigoReporte = LeerEntero("Código del partido", 1, int.MaxValue);
                Console.WriteLine();

                var reporte = await servicioTicketPremium.ObtenerResumenVentas(codigoReporte);
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("       REPORTE DE VENTAS");
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine($"  Partido: {reporte.Partido}");
                Console.WriteLine($"  Fecha:   {reporte.Fecha}");
                Console.WriteLine("───────────────────────────────────────");
                Console.WriteLine($"  {"Localidad",-20} {"Vend.",6} {"Total",10}");
                Console.WriteLine("  " + new string('-', 38));

                foreach (var det in reporte.Detalles)
                {
                    Console.WriteLine($"  {det.Localidad,-20} {det.Vendidos,6} {det.TotalRecaudado,10:F2}");
                }

                Console.WriteLine("═══════════════════════════════════════");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener reporte: {ex.Message}");
            }
            EsperarContinuacion();
            break;
        case 0:
            return;
    }
}
