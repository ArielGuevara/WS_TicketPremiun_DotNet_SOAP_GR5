namespace WS_TicketPremium_CLI_MOV_ESC.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnIniciarSesion(object? sender, EventArgs e)
        {
            var correo = TxtCorreo.Text?.Trim();
            var password = TxtPassword.Text;

            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(password))
            {
                MostrarMensaje("Correo y contraseña son obligatorios.", Colors.Red);
                return;
            }

            Cargando(true);
            try
            {
                var usuario = await AppServices.TicketPremium.IniciarSesion(correo, password);
                if (usuario is not null)
                {
                    await App.SetUsuarioAsync(usuario);
                    MostrarMensaje($"Bienvenido, {usuario.Nombres}.", Colors.Green);
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    MostrarMensaje("Credenciales inválidas.", Colors.Red);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", Colors.Red);
            }
            finally
            {
                Cargando(false);
            }
        }

        private void Cargando(bool activo)
        {
            LoadingIndicator.IsVisible = activo;
            LoadingIndicator.IsRunning = activo;
            BtnIniciarSesion.IsEnabled = !activo;
        }

        private void MostrarMensaje(string mensaje, Color color)
        {
            LblMensaje.Text = mensaje;
            LblMensaje.TextColor = color;
            LblMensaje.IsVisible = true;
        }
    }
}
