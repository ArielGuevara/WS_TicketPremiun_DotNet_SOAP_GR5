using Compartido.Servicios.Negocio;

namespace WS_TicketPremium_CLI_MOV_ESC
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnPointerEntered(object? sender, PointerEventArgs e)
        {
            if (sender is Border border)
                border.BackgroundColor = Color.FromArgb("#E3F2FD");
        }

        private void OnPointerExited(object? sender, PointerEventArgs e)
        {
            if (sender is Border border)
                border.BackgroundColor = Color.FromArgb("#FFFFFF");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ActualizarEstadoSesion();
        }

        private async void ActualizarEstadoSesion()
        {
            var usuario = await App.GetUsuarioAsync();
            if (usuario is not null)
            {
                LblIconoSesion.Text = "✓";
                LblEstadoSesion.Text = $"{usuario.Nombres} ({usuario.Correo})";
                LblEstadoSesion.TextColor = Color.FromArgb("#2E7D32");
                BorderIniciarSesion.IsVisible = false;
                BorderRegistrar.IsVisible = false;
                BorderCerrarSesion.IsVisible = true;
            }
            else
            {
                LblIconoSesion.Text = "👤";
                LblEstadoSesion.Text = "No has iniciado sesión";
                LblEstadoSesion.TextColor = Color.FromArgb("#6B7280");
                BorderIniciarSesion.IsVisible = true;
                BorderRegistrar.IsVisible = true;
                BorderCerrarSesion.IsVisible = false;
            }
        }

        private async void OnIniciarSesion(object? sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync("login");
        }

        private async void OnRegistrar(object? sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync("registro");
        }

        private async void OnVerPartidos(object? sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync("partidos");
        }

        private async void OnVerReporte(object? sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync("reporte");
        }

        private async void OnCerrarSesion(object? sender, TappedEventArgs e)
        {
            try
            {
                var usuario = await App.GetUsuarioAsync();
                if (usuario is not null)
                {
                    await AppServices.TicketPremium.CerrarSesion(usuario.TokenSession);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cerrar sesión: {ex.Message}", "OK");
            }
            finally
            {
                App.LimpiarSesion();
                ActualizarEstadoSesion();
            }
        }
    }
}
