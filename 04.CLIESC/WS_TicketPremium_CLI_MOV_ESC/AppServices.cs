using Compartido.Servicios.Negocio;

namespace WS_TicketPremium_CLI_MOV_ESC
{
    public static class AppServices
    {
        private static readonly string _host;
        private static readonly ServicioTicketPremium _ticketPremium;
        private static readonly ServicioFederacion _federacion;

        static AppServices()
        {
            _host = DeviceInfo.Platform == DevicePlatform.Android ? "10.0.2.2" : "localhost";
            _ticketPremium = new ServicioTicketPremium($"http://{_host}:52768/TicketPremiumService.svc");
            _federacion = new ServicioFederacion($"http://{_host}:60235/FederacionService.svc");
        }

        public static ServicioTicketPremium TicketPremium => _ticketPremium;
        public static ServicioFederacion Federacion => _federacion;
    }
}
