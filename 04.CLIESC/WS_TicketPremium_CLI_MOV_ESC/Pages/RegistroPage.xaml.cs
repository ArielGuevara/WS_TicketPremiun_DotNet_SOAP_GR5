namespace WS_TicketPremium_CLI_MOV_ESC.Pages
{
    public partial class RegistroPage : ContentPage
    {
        public RegistroPage()
        {
            InitializeComponent();
        }

        private async void OnRegistrar(object? sender, EventArgs e)
        {
            var nombres = TxtNombres.Text?.Trim();
            var correo = TxtCorreo.Text?.Trim();
            var password = TxtPassword.Text;

            if (string.IsNullOrWhiteSpace(nombres) || string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(password))
            {
                MostrarMensaje("Todos los campos son obligatorios.", Colors.Red);
                return;
            }

            Cargando(true);
            try
            {
                var registrado = await AppServices.TicketPremium.RegistrarUsuario(nombres, correo, password);
                if (registrado)
                {
                    MostrarMensaje("Usuario registrado correctamente.", Colors.Green);
                    await Task.Delay(1000);
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    MostrarMensaje("El correo ya está registrado.", Colors.Red);
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
            BtnRegistrar.IsEnabled = !activo;
        }

        private void MostrarMensaje(string mensaje, Color color)
        {
            LblMensaje.Text = mensaje;
            LblMensaje.TextColor = color;
            LblMensaje.IsVisible = true;
            LblMensaje.FontSize = 13;
        }
    }
}
