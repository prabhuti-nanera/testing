using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Http;
using Blazored.LocalStorage;
using CRC.WebPortal.BlazorWebApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Completely disable all logging
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.None);

// Set root components for pure WebAssembly
builder.RootComponents.Add<CRC.WebPortal.BlazorWebApp.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient for API communication
builder.Services.AddHttpClient("API", client => client.BaseAddress = new Uri("http://localhost:5103/"));
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));

// Add Blazored LocalStorage for token storage
builder.Services.AddBlazoredLocalStorage();

// Register services - CQRS Pattern: Service registration for dependency injection
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());

await builder.Build().RunAsync();
