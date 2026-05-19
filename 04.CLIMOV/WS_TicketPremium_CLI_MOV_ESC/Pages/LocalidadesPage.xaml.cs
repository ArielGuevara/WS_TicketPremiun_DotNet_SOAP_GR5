using Compartido.Modelos.Negocio;

namespace WS_TicketPremium_CLI_MOV_ESC.Pages
{
    [QueryProperty(nameof(CodigoPartido), "codigoPartido")]
    public partial class LocalidadesPage : ContentPage
    {
        private PartidoDTO? _partido;
        private LocalidadDTO? _localidadSeleccionada;
        private List<LocalidadDTO> _localidades = [];
        private bool _paginaCargada;

        private int _codigoPartido;
        public int CodigoPartido
        {
            get => _codigoPartido;
            set
            {
                _codigoPartido = value;
                if (_paginaCargada)
                    _ = CargarDatos();
            }
        }

        public LocalidadesPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error init LocalidadesPage: {ex}");
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!_paginaCargada)
            {
                _paginaCargada = true;
                await CargarDatos();
            }
        }

        private async Task CargarDatos()
        {
            try
            {
                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;

                var partidos = await AppServices.Federacion.ObtenerPartidosDisponibles();
                _partido = partidos.FirstOrDefault(p => p.Codigo == CodigoPartido);
                LblTitulo.Text = _partido is not null
                    ? $"{_partido.EquipoLocal} vs {_partido.EquipoVisita}"
                    : $"Partido #{CodigoPartido}";

                _localidades = await AppServices.Federacion.ObtenerLocalidadesDisponibles(CodigoPartido);
                ListaLocalidades.ItemsSource = _localidades;
                ListaLocalidades.IsVisible = _localidades.Count > 0;
                StackVacio.IsVisible = _localidades.Count == 0;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudieron cargar las localidades: {ex.Message}", "OK");
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
                border.BackgroundColor = Color.FromArgb("#F0F4F8");
        }

        private void OnItemPointerExited(object? sender, PointerEventArgs e)
        {
            if (sender is Border border)
                border.BackgroundColor = Color.FromArgb("#FFFFFF");
        }

        private void OnCompraPointerEntered(object? sender, PointerEventArgs e)
        {
            if (sender is Border border)
                border.BackgroundColor = Color.FromArgb("#F0F4F8");
        }

        private void OnCompraPointerExited(object? sender, PointerEventArgs e)
        {
            if (sender is Border border)
                border.BackgroundColor = Color.FromArgb("#FFFFFF");
        }

        private int? ObtenerCantidad()
        {
            if (int.TryParse(TxtCantidad.Text, out var c) && c >= 1)
                return c;
            return null;
        }

        private void OnLocalidadTapped(object? sender, TappedEventArgs e)
        {
            try
            {
                if (sender is not Border border) return;
                if (border.BindingContext is not LocalidadDTO loc) return;

                _localidadSeleccionada = loc;
                LblLocalidadSel.Text = $"📍 {loc.CodigoLocalidad} — ${loc.Precio:F2} c/u";
                TxtCantidad.Text = "1";
                ActualizarTotal();
                BorderCompra.IsVisible = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error seleccion localidad: {ex}");
            }
        }

        private void OnCantidadTextoChanged(object? sender, TextChangedEventArgs e)
        {
            ActualizarTotal();
        }

        private void ActualizarTotal()
        {
            if (_localidadSeleccionada is null) return;
            var cantidad = ObtenerCantidad() ?? 1;
            var subtotal = cantidad * _localidadSeleccionada.Precio;
            var iva = subtotal * 0.15m;
            LblTotal.Text = $"Total: ${subtotal + iva:F2} (IVA incl.)";
        }

        private async void OnComprar(object? sender, EventArgs e)
        {
            if (_localidadSeleccionada is null) return;

            var cantidad = ObtenerCantidad();
            if (cantidad is null)
            {
                LblMensaje.Text = "Ingresa una cantidad válida (número entero positivo).";
                LblMensaje.TextColor = Color.FromArgb("#C62828");
                LblMensaje.IsVisible = true;
                return;
            }
            if (cantidad > _localidadSeleccionada.Disponibilidad)
            {
                LblMensaje.Text = $"Solo hay {_localidadSeleccionada.Disponibilidad} boleto(s) disponible(s).";
                LblMensaje.TextColor = Color.FromArgb("#C62828");
                LblMensaje.IsVisible = true;
                return;
            }

            var usuario = await App.GetUsuarioAsync();
            if (usuario is null)
            {
                LblMensaje.Text = "Debes iniciar sesión para comprar.";
                LblMensaje.TextColor = Color.FromArgb("#C62828");
                LblMensaje.IsVisible = true;
                return;
            }

            try
            {
                BorderCompra.IsEnabled = false;
                var factura = await AppServices.TicketPremium.ComprarBoletos(
                    usuario.IdUsuario, CodigoPartido,
                    _localidadSeleccionada.CodigoLocalidad,
                    cantidad.Value, _localidadSeleccionada.Precio);

                if (factura.IdFactura > 0)
                {
                    MostrarFacturaPopup(factura);
                }
                else
                {
                    LblMensaje.Text = factura.Mensaje;
                    LblMensaje.TextColor = Color.FromArgb("#C62828");
                    LblMensaje.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                LblMensaje.Text = $"Error en la compra: {ex.Message}";
                LblMensaje.TextColor = Color.FromArgb("#C62828");
                LblMensaje.IsVisible = true;
            }
            finally
            {
                BorderCompra.IsEnabled = true;
            }
        }

        private void MostrarFacturaPopup(FacturaDTO factura)
        {
            FacturaNumero.Text = $"🧾 Factura #{factura.IdFactura}";
            FacturaSubtotal.Text = $"${factura.Subtotal:F2}";
            FacturaIva.Text = $"${factura.Iva:F2}";
            FacturaTotal.Text = $"${factura.TotalFinal:F2}";
            FacturaMensaje.Text = $"✅ {factura.Mensaje}";
            OverlayFactura.IsVisible = true;
        }

        private async void OnCerrarFactura(object? sender, EventArgs e)
        {
            OverlayFactura.IsVisible = false;
            await Shell.Current.GoToAsync("../..");
        }
    }
}
