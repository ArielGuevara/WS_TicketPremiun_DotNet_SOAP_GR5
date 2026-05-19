using Compartido.Modelos.Negocio;
using Compartido.Servicios.Negocio;

static void Escribir(string texto, ConsoleColor? color = null)
{
    if (color.HasValue) Console.ForegroundColor = color.Value;
    Console.Write(texto);
    Console.ResetColor();
}

static void EscribirLinea(string texto, ConsoleColor? color = null)
{
    if (color.HasValue) Console.ForegroundColor = color.Value;
    Console.WriteLine(texto);
    Console.ResetColor();
}

static void DibujarSulliDerecha()
{
    var ancho = Console.WindowWidth;
    var inicioX = Math.Max(ancho - 88, 0);
    var colorAnterior = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Magenta;

    var lineas = new[]
    {
        "    .              .                         .    .                   ",
        "                     .    .     ..              .    . .         .    ",
        "       .     .                  ===:...                               ",
        " .                             #.-:::+=..              .         .    ",
        "              .             . -*+--:.   --:..                       . ",
        "        .   .                  @*++--=*=*+===-:.                      ",
        "=-::----:-:-.  .             .=@%#+%#*****+*===---.       .           ",
        "%*+-------+*##*#*=-:.  .: .=#%%%@%@%%####**++*==*====--:::.           ",
        "%##+--=--=*###%@@##***%@%%#%%%#%%%%@%%%%%%#%%%*++=-=***++=:           ",
        "###*+=----+=+*%#*+**########*++=-====**%%*+*%****=*+++*#%%+        .  ",
        "%%+==-----==+******##########**+----:-#: -.=--=-=#%#*+=**:+        .  ",
        "***+-------++=*#**###########**+=----:::#@@..+==####*++=+%#. .        ",
        "**#+=+---=-===*#######%######**+------::.=.  :=**##*#+=*==-.          ",
        "+++==----=-=+**####%#######*##+==-------::-----@@=.##*#+==-.          ",
        "===----++=-=+*########*+++==*#%------:-##+----=*@= .#%#%*+:   .       ",
        "=++==++==+**#%%%#%#***++===+%=@#=------+@@*=--::   ###:   .          .",
        "=+==++*#+++*%%%######*+====+%+@*+*---::-:%@*=---::--:             .   ",
        "++*++***#%%%%%#####**+==+==+#**@@-%=--:::::##%@*:::-.     ..          ",
        "++*+**##%%%%%%#####****+===+*#-:  -#%------:::::::--                  ",
        "*****+*#%%%%%#######***+=--=+*%- .-@.=%+:-:::::::--:.                 ",
        "*+**#*#%%%%%%######**#+++=--=+*#--: +@+=%*:::::::--              .    ",
        "+***#%%%%%%%%#######**#*=+=---==+=---.-@@#*%=-----              .     ",
        "*####%%%%%%%%########*#*++-------=+=------=.*----.                    ",
        "*###%%#%%%%%%%#########**++---------==----------:    .                ",
        "*##%%%%%%%%%%%%%%#######**++-=-----------------:             .      . ",
        "###%%%%%%%%%%%%%%%#%###***+=+=----------------.                        ",
        "###%%%%%%%%%%%%%%%%#######**+==--------------.                      . ",
        "####%%%%%%%%%%%%%%%%%%###**#*++=------------.  .     .   .   .    . . ",
        "#%%%##%%%%%%%%%%%%%%%%%%###**+++==--------: .            .            ",
        "####%%%##%%%%%%%%%%%%%%%%%###***+++==--:.                             ",
        "#######%%%#%%%%%%%%%%%%%%%####*#**++=::.                  .           ",
        "*#####%%%%%%%%%%%%%#######**+++=-:--------*+:                         ",
        "%%%####*##%#%%########*+===----:-----:-=+*-                            ",
        "@@%%%%%#**=+++*++++#%#*=-------------::::=:...         .           .  ",
        "%%%@@%+*==+====++#@@%%%#===-----------::::=-.. .. .          .    ..  ",
        "@@%@%*++*++*+++*#@@%%@%%*++-=-------------=-:.........             .  ",
        "@%#%#*+*#**+*###%@@%@@%%*+==-------:--==--    .... .   .      ....    ",
        "++***###+***++*%@@@%@@%%*==-----------:::. .  .....:+*:       .       ",
        "**#=***+#****##@%@@@@@#*+=--------==---:::      ...:===        .      ",
        "#++++*+**#*#+**#%%%%%#*++==+------===---::      ...:---               ",
        "+=**+**+***##**####****==+++=-------+*==-. .   ........        .      ",
        "*###***+*##****+***++=+++===+---------***:   ....:....                ",
        "%@#****#*#**#****++++*+++=+++=--------===.....:.::...                 ",
        "@%###*+**+**#*#*#*++**#*+-:. #*+=-----+-...::::.....                  ",
        "@%@@*+*##*****#**+***-  .   .=#*++==:.=-......:::     .        .      ",
        "@%%#*##%%#%###%##*=.   . .         .                                  ",
        "%@%%%%%%%%%%%%%%%=  .                          ..                .    ",
        "%%######%%##%%%#-          . .      .             .   .               ",
        "*********######*     .     .                                          ",
        "+++++++++******+      .  .                                        ..  ",
        "-=-=====+++++*+                                                        ",
        "--------===+++.                     ..                  .       .     ",
        "--:-------==+                                .                        ",
        "--:--:------..                          .        .                    ",
        "-::--:-----..    .        .                  .                        ",
        "-:--:-::--.                                  .                        ",
        "-::-------                       .              .                     ",
        "-:-----:--:       .                    .     ..        .              ",
        "-:---------:    .   .                      . .      .                 ",
        "-------=-==-                      .      .                            ",
        "----=+*%%##*:                              .                          ",
        "======#%%##**.                                 .    .          .      ",
        "=+=====*%#***=.                          .                            ",
    };

    for (var i = 0; i < lineas.Length; i++)
    {
        var y = i;
        if (y >= Console.WindowHeight) break;
        Console.SetCursorPosition(inicioX, y);
        var linea = lineas[i];
        var maxLen = ancho - inicioX;
        if (linea.Length > maxLen)
            linea = linea.Substring(0, maxLen);
        Console.Write(linea);
    }

    Console.ForegroundColor = colorAnterior;
    Console.SetCursorPosition(0, 0);
}

static void MostrarEncabezado()
{
    Console.Clear();
    DibujarSulliDerecha();
    Console.SetCursorPosition(0, 0);
    Console.WriteLine();
    EscribirLinea("  ================================================", ConsoleColor.Blue);
    Escribir("  |", ConsoleColor.Blue);
    Console.Write("             ");
    Escribir("TICKET PREMIUM MONSTER    ", ConsoleColor.Cyan);
    Console.Write("       ");
    EscribirLinea("|", ConsoleColor.Blue);
    Escribir("  |", ConsoleColor.Blue);
    Console.Write("        ");
    Escribir("Boletos para partidos de futbol    ", ConsoleColor.DarkCyan);
    Console.Write("   ");
    EscribirLinea("|", ConsoleColor.Blue);
    EscribirLinea("  ================================================", ConsoleColor.Blue);
    Console.WriteLine();
}

static string LeerTexto(string etiqueta)
{
    Console.Write("  > ");
    Escribir($"{etiqueta}: ", ConsoleColor.Yellow);
    return Console.ReadLine()?.Trim() ?? string.Empty;
}

static int LeerEntero(string etiqueta, int minimo, int maximo)
{
    while (true)
    {
        Console.Write("  > ");
        Escribir($"{etiqueta}: ", ConsoleColor.Yellow);
        var entrada = Console.ReadLine();
        if (int.TryParse(entrada, out var valor) && valor >= minimo && valor <= maximo)
            return valor;
        EscribirLinea($"  Valor inválido. Debe ser entre {minimo} y {maximo}.", ConsoleColor.Red);
    }
}

static async Task MostrarPartidosAsync(ServicioFederacion servicioFederacion)
{
    var partidos = await servicioFederacion.ObtenerPartidosDisponibles();
    if (partidos.Count == 0)
    {
        EscribirLinea("  No hay partidos disponibles.", ConsoleColor.DarkYellow);
        return;
    }

    for (var i = 0; i < partidos.Count; i++)
    {
        var p = partidos[i];
        Console.Write($"  {i + 1}.");
        Escribir($" {p.EquipoLocal}", ConsoleColor.White);
        Console.Write(" vs");
        Escribir($" {p.EquipoVisita}", ConsoleColor.White);
        Console.WriteLine();
        Console.Write($"     ");
        Escribir($"{p.Fecha:dd/MM/yyyy HH:mm}", ConsoleColor.DarkGray);
        Console.Write("  -  ");
        Escribir($"{p.Lugar}", ConsoleColor.DarkGray);
        Console.WriteLine();
        Console.WriteLine();
    }
}

static async Task<List<LocalidadDTO>> MostrarLocalidadesAsync(ServicioFederacion servicioFederacion, int codigoPartido)
{
    var localidades = await servicioFederacion.ObtenerLocalidadesDisponibles(codigoPartido);
    if (localidades.Count == 0)
    {
        EscribirLinea("  No hay localidades disponibles.", ConsoleColor.DarkYellow);
        return localidades;
    }

    var idx = 1;
    foreach (var loc in localidades)
    {
        Console.Write($"  {idx,2}.");
        Escribir($" {loc.CodigoLocalidad,-16}", ConsoleColor.White);
        Escribir($"  Disponibles:", ConsoleColor.DarkGray);
        Escribir($" {loc.Disponibilidad,4}", ConsoleColor.DarkYellow);
        Escribir($"  Precio:", ConsoleColor.DarkGray);
        EscribirLinea($" ${loc.Precio,6:F2}", ConsoleColor.Green);
        idx++;
    }

    return localidades;
}

static void EsperarContinuacion()
{
    Console.WriteLine();
    DibujarSulliDerecha();
    Console.SetCursorPosition(0, Console.CursorTop);
    Escribir("Presiona ENTER para continuar...", ConsoleColor.DarkGray);
    Console.ReadLine();
}

static void MostrarSeparador()
{
    EscribirLinea("  " + new string('-', 40), ConsoleColor.DarkBlue);
}

// ── Configuración ──
var urlTicketPremium = "http://localhost:52768/TicketPremiumService.svc";
var urlFederacion = "http://localhost:60235/FederacionService.svc";

var servicioTicketPremium = new ServicioTicketPremium(urlTicketPremium);
var servicioFederacion = new ServicioFederacion(urlFederacion);

UsuarioDTO? usuarioActual = null;

Console.Clear();
DibujarSulliDerecha();
Console.SetCursorPosition(0, 0);
EscribirLinea("Bienvenido a Ticket Premium Monster!", ConsoleColor.Cyan);
EsperarContinuacion();

while (true)
{
    MostrarEncabezado();

    if (usuarioActual is not null)
    {
        Escribir("  [OK] ", ConsoleColor.Green);
        EscribirLinea($"{usuarioActual.Nombres} ({usuarioActual.Correo})", ConsoleColor.White);
        Console.WriteLine();
    }

    MostrarSeparador();
    Console.WriteLine("  [ MENU PRINCIPAL ]");
    MostrarSeparador();
    Console.WriteLine();
    Console.WriteLine("  [1]  Iniciar sesion");
    Console.WriteLine("  [2]  Registrar usuario");
    Console.WriteLine("  [3]  Ver partidos disponibles");
    Console.WriteLine("  [4]  Ver localidades");
    Console.WriteLine("  [5]  Comprar boletos");
    Console.WriteLine("  [6]  Cerrar sesion");
    Console.WriteLine("  [7]  Reporte de ventas");
    Console.WriteLine();
    Console.WriteLine("  [0]  Salir");
    Console.WriteLine();
    MostrarSeparador();
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
                if (usuarioActual is null)
                    EscribirLinea("  [ERROR] Credenciales invalidas.", ConsoleColor.Red);
                else
                {
                    Escribir("  [OK] Bienvenido, ", ConsoleColor.Green);
                    EscribirLinea($"{usuarioActual.Nombres}!", ConsoleColor.White);
                }
            }
            catch (Exception ex)
            {
                EscribirLinea($"  [ERROR] Error al iniciar sesion: {ex.Message}", ConsoleColor.Red);
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
                EscribirLinea(registrado
                    ? "  [OK] Usuario registrado correctamente."
                    : "  [ERROR] El correo ya está registrado.", registrado ? ConsoleColor.Green : ConsoleColor.Red);
            }
            catch (Exception ex)
            {
                EscribirLinea($"  [ERROR] Error al registrar: {ex.Message}", ConsoleColor.Red);
            }
            EsperarContinuacion();
            break;

        case 3:
            MostrarEncabezado();
            try
            {
                EscribirLinea("  PARTIDOS DISPONIBLES", ConsoleColor.Cyan);
                MostrarSeparador();
                await MostrarPartidosAsync(servicioFederacion);
            }
            catch (Exception ex)
            {
                EscribirLinea($"  [ERROR] Error al obtener partidos: {ex.Message}", ConsoleColor.Red);
            }
            EsperarContinuacion();
            break;

        case 4:
            MostrarEncabezado();
            try
            {
                EscribirLinea("  PARTIDOS DISPONIBLES", ConsoleColor.Cyan);
                MostrarSeparador();
                await MostrarPartidosAsync(servicioFederacion);
                var codigoPartido = LeerEntero("Código del partido", 1, int.MaxValue);
                Console.WriteLine();
                EscribirLinea("  LOCALIDADES", ConsoleColor.Cyan);
                MostrarSeparador();
                await MostrarLocalidadesAsync(servicioFederacion, codigoPartido);
            }
            catch (Exception ex)
            {
                EscribirLinea($"  [ERROR] Error al obtener localidades: {ex.Message}", ConsoleColor.Red);
            }
            EsperarContinuacion();
            break;

        case 5:
            MostrarEncabezado();
            if (usuarioActual is null)
            {
                EscribirLinea("  Debes iniciar sesión para comprar.", ConsoleColor.Yellow);
                EsperarContinuacion();
                break;
            }

            try
            {
                EscribirLinea("  PARTIDOS DISPONIBLES", ConsoleColor.Cyan);
                MostrarSeparador();
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
                Console.WriteLine();
                EscribirLinea("  Procesando compra...", ConsoleColor.DarkGray);

                var factura = await servicioTicketPremium.ComprarBoletos(
                    usuarioActual.IdUsuario, partidoCompra, loc.CodigoLocalidad, cantidad, loc.Precio);

                Console.WriteLine();
                if (factura.IdFactura > 0)
                {
                    MostrarSeparador();
                    EscribirLinea("  COMPRA EXITOSA  ", ConsoleColor.Green);
                    MostrarSeparador();
                    EscribirLinea($"  Factura #{factura.IdFactura}", ConsoleColor.Cyan);
                    Console.WriteLine();
                    Console.Write($"    Subtotal:        ");
                    EscribirLinea($"${factura.Subtotal,8:F2}", ConsoleColor.White);
                    Console.Write($"    IVA (15%):       ");
                    EscribirLinea($"${factura.Iva,8:F2}", ConsoleColor.White);
                    MostrarSeparador();
                    Console.Write($"    TOTAL:           ");
                    EscribirLinea($"${factura.TotalFinal,8:F2}", ConsoleColor.Green);
                    MostrarSeparador();
                    EscribirLinea($"  {factura.Mensaje}", ConsoleColor.DarkGray);
                }
                else
                {
                    EscribirLinea($"  [ERROR] {factura.Mensaje}", ConsoleColor.Red);
                }
            }
            catch (Exception ex)
            {
                EscribirLinea($"  [ERROR] Error al comprar boletos: {ex.Message}", ConsoleColor.Red);
            }
            EsperarContinuacion();
            break;

        case 6:
            MostrarEncabezado();
            if (usuarioActual is null)
            {
                EscribirLinea("  No hay sesión activa.", ConsoleColor.Yellow);
                EsperarContinuacion();
                break;
            }

            try
            {
                var cerrado = await servicioTicketPremium.CerrarSesion(usuarioActual.TokenSession);
                usuarioActual = null;
                EscribirLinea(cerrado ? "  [OK] Sesión cerrada." : "  [ERROR] No se pudo cerrar la sesión.",
                    cerrado ? ConsoleColor.Green : ConsoleColor.Red);
            }
            catch (Exception ex)
            {
                EscribirLinea($"  [ERROR] Error al cerrar sesión: {ex.Message}", ConsoleColor.Red);
            }
            EsperarContinuacion();
            break;

        case 7:
            MostrarEncabezado();
            try
            {
                EscribirLinea("  PARTIDOS DISPONIBLES", ConsoleColor.Cyan);
                MostrarSeparador();
                await MostrarPartidosAsync(servicioFederacion);
                Console.WriteLine();
                var codigoReporte = LeerEntero("Código del partido", 1, int.MaxValue);
                Console.WriteLine();

                var reporte = await servicioTicketPremium.ObtenerResumenVentas(codigoReporte);
                MostrarSeparador();
                EscribirLinea("  REPORTE DE VENTAS", ConsoleColor.Cyan);
                MostrarSeparador();
                Console.WriteLine($"  Partido: {reporte.Partido}");
                Console.WriteLine($"  Fecha:   {reporte.Fecha}");
                MostrarSeparador();
                Console.Write($"  {"Localidad",-20}");
                Console.Write($" {"Vend.",6}");
                EscribirLinea($" {"Total",10}", ConsoleColor.Green);
                MostrarSeparador();

                decimal totalGeneral = 0;
                foreach (var det in reporte.Detalles)
                {
                    Console.Write($"  {det.Localidad,-20}");
                    Console.Write($" {det.Vendidos,6}");
                    EscribirLinea($" {det.TotalRecaudado,10:F2}", ConsoleColor.Green);
                    totalGeneral += det.TotalRecaudado;
                }

                MostrarSeparador();
                Console.Write($"  {"TOTAL GENERAL",-20}");
                EscribirLinea($" {totalGeneral,16:F2}", ConsoleColor.Cyan);
                MostrarSeparador();
            }
            catch (Exception ex)
            {
                EscribirLinea($"  [ERROR] Error al obtener reporte: {ex.Message}", ConsoleColor.Red);
            }
            EsperarContinuacion();
            break;

        case 0:
            EscribirLinea("\n  ¡Gracias por usar Ticket Premium Monster!", ConsoleColor.Cyan);
            return;
    }
}
