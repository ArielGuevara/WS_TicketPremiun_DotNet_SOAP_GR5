using Compartido.Modelos.Negocio;
using Compartido.Servicios.Negocio;

namespace WS_TicketPremium_CLI_WEB.Services;

public class AppState
{
    public UsuarioDTO? UsuarioActual { get; set; }
    public bool EstaLogeado => UsuarioActual is not null;

    public event Action? OnCambio;

    public void IniciarSesion(UsuarioDTO usuario)
    {
        UsuarioActual = usuario;
        OnCambio?.Invoke();
    }

    public void CerrarSesion()
    {
        UsuarioActual = null;
        OnCambio?.Invoke();
    }
}
