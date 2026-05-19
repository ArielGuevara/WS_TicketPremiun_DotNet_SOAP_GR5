using Compartido.Modelos.Negocio;

namespace WS_TicketPremium_CLI_MOV_ESC.Pages
{
    public partial class ReporteVentasPage : ContentPage
    {
        public ReporteVentasPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarPartidos();
        }

        private async Task CargarPartidos()
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            try
            {
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

        private async void OnPartidoTapped(object? sender, TappedEventArgs e)
        {
            try
            {
                if (sender is not Border border) return;
                if (border.BindingContext is not PartidoDTO partido) return;

                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;

                var reporte = await AppServices.TicketPremium.ObtenerResumenVentas(partido.Codigo);
                MostrarReporte(reporte);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al obtener reporte: {ex.Message}", "OK");
            }
            finally
            {
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
            }
        }

        private void MostrarReporte(ReporteResumenVentasDTO reporte)
        {
            RepPartido.Text = reporte.Partido;
            RepFecha.Text = reporte.Fecha;
            StackDetalles.Children.Clear();

            decimal totalGeneral = 0;

            foreach (var det in reporte.Detalles)
            {
                var fila = new Grid
                {
                    ColumnDefinitions =
                    [
                        new ColumnDefinition(GridLength.Star),
                        new ColumnDefinition(GridLength.Auto),
                        new ColumnDefinition(GridLength.Auto)
                    ],
                    ColumnSpacing = 16
                };

                fila.Add(new Label
                {
                    Text = det.Localidad,
                    FontSize = 14,
                    TextColor = Color.FromArgb("#1F2937"),
                    VerticalOptions = LayoutOptions.Center
                }, 0);

                fila.Add(new Label
                {
                    Text = det.Vendidos.ToString(),
                    FontSize = 14,
                    TextColor = Color.FromArgb("#1F2937"),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.End
                }, 1);

                fila.Add(new Label
                {
                    Text = $"${det.TotalRecaudado:F2}",
                    FontSize = 14,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#1565C0"),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.End
                }, 2);

                StackDetalles.Children.Add(fila);
                totalGeneral += det.TotalRecaudado;
            }

            RepTotalGeneral.Text = $"${totalGeneral:F2}";
            OverlayReporte.IsVisible = true;
        }

        private async void OnCerrarReporte(object? sender, EventArgs e)
        {
            OverlayReporte.IsVisible = false;
            await Shell.Current.GoToAsync("..");
        }
    }
}
