using Compartido.Modelos.Negocio;

namespace WS_TicketPremium_CLI_MOV_ESC.Pages
{
    public partial class PartidosPage : ContentPage
    {
        public PartidosPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error init PartidosPage: {ex}");
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarPartidos();
        }

        private async Task CargarPartidos()
        {
            try
            {
                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;

                var partidos = await AppServices.Federacion.ObtenerPartidosDisponibles();
                ListaPartidos.ItemsSource = partidos;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudieron cargar los partidos: {ex.Message}", "OK");
            }
            finally
            {
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
            }
        }

        private void OnItemPointerEntered(object? sender, PointerEventArgs e)
        {
            if (sender is Border border)
                border.BackgroundColor = Color.FromArgb("#E3F2FD");
        }

        private void OnItemPointerExited(object? sender, PointerEventArgs e)
        {
            if (sender is Border border)
                border.BackgroundColor = Color.FromArgb("#FFFFFF");
        }

        private async void OnItemTapped(object? sender, TappedEventArgs e)
        {
            try
            {
                if (sender is not Border border) return;
                if (border.BindingContext is not PartidoDTO partido) return;

                await Shell.Current.GoToAsync($"localidades?codigoPartido={partido.Codigo}");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
