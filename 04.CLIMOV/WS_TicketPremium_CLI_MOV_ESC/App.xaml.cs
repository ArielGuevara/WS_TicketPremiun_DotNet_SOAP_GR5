using Compartido.Modelos.Negocio;

namespace WS_TicketPremium_CLI_MOV_ESC
{
    public partial class App : Application
    {
        private static UsuarioDTO? _usuarioActual;

        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        public static Task<UsuarioDTO?> GetUsuarioAsync()
        {
            return Task.FromResult(_usuarioActual);
        }

        public static Task SetUsuarioAsync(UsuarioDTO usuario)
        {
            _usuarioActual = usuario;
            return Task.CompletedTask;
        }

        public static void LimpiarSesion()
        {
            _usuarioActual = null;
        }
    }
}
