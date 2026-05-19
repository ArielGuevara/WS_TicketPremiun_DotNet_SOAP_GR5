using Microsoft.Extensions.Logging;

namespace WS_TicketPremium_CLI_MOV_ESC
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", true);

            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
