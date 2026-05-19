using Compartido.Servicios.Negocio;
using WS_TicketPremium_CLI_WEB.Components;
using WS_TicketPremium_CLI_WEB.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton(new ServicioTicketPremium("http://localhost:52768/TicketPremiumService.svc"));
builder.Services.AddSingleton(new ServicioFederacion("http://localhost:60235/FederacionService.svc"));
builder.Services.AddSingleton<AppState>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found");
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
