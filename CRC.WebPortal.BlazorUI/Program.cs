using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using FluentValidation;
using CRC.WebPortal.BlazorUI;
using CRC.WebPortal.BlazorUI.Services;
using CRC.WebPortal.BlazorUI.Validators;
using CRC.WebPortal.BlazorUI.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

#region Component Registration

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

#endregion

#region HTTP Client Configuration

// Configure HttpClient for API communication
var apiBaseUrl = builder.Configuration.GetValue<string>("ApiBaseUrl") ?? "https://localhost:7000";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

#endregion

#region Storage Services

// Add Blazored LocalStorage for token storage
builder.Services.AddBlazoredLocalStorage();

#endregion

#region Authentication Services

// Add Authentication services
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();

#endregion

#region UI Validation Services

// UI Validation - This is UI concern and belongs in BlazorUI layer
builder.Services.AddScoped<IValidator<SignUpRequest>, SignUpRequestValidator>();
builder.Services.AddScoped<IValidator<SignInRequest>, SignInRequestValidator>();

#endregion

await builder.Build().RunAsync();
