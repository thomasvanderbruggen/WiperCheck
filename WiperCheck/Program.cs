using WiperCheck.Components;
using WiperCheck.Services.DateTime;
using WiperCheck.Services.Geocoding;
using WiperCheck.Services.Routing;
using WiperCheck.Services.TripForecast;
using WiperCheck.Services.Weather;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient<GeocodeService>(client =>
{
    client.BaseAddress = new Uri("https://api.openrouteservice.org/geocode/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddHttpClient<RoutingService>(client =>
{
    client.BaseAddress = new Uri("https://api.openrouteservice.org/");
    client.DefaultRequestHeaders.Add("Authorization", builder.Configuration["ORS:ApiKey"]);
});
builder.Services.AddHttpClient<WeatherService>(client =>
{
    client.BaseAddress = new Uri("https://api.open-meteo.com/v1/forecast");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddScoped<TripForecastService>();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
