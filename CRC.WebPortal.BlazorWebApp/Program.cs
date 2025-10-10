using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using CRC.WebPortal.BlazorWebApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Set root components for pure WebAssembly
builder.RootComponents.Add<CRC.WebPortal.BlazorWebApp.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient for API communication
builder.Services.AddHttpClient("API", client => client.BaseAddress = new Uri("http://localhost:5103/"));
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));

// Add Blazored LocalStorage for token storage
builder.Services.AddBlazoredLocalStorage();

// Add Authentication services
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();

await builder.Build().RunAsync();
