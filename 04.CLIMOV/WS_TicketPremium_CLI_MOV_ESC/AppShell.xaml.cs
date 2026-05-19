using WS_TicketPremium_CLI_MOV_ESC.Pages;

namespace WS_TicketPremium_CLI_MOV_ESC
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("registro", typeof(RegistroPage));
            Routing.RegisterRoute("partidos", typeof(PartidosPage));
            Routing.RegisterRoute("localidades", typeof(LocalidadesPage));
            Routing.RegisterRoute("reporte", typeof(ReporteVentasPage));
        }
    }
}
