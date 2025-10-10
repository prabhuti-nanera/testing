using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.BlazorWebApp.Models;

namespace CRC.WebPortal.BlazorWebApp.Services;

/// <summary>
/// AuthService follows Clean Architecture - UI only sends data, backend creates commands
/// </summary>
public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly IJSRuntime _jsRuntime;
    private const string TokenKey = "authToken";

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Sends signup data to API - API creates the command internally
    /// </summary>
    public async Task<AuthResponse> SignUpAsync(SignupData signupData)
    {
        try
        {
            // Log to both server console and browser console
            Console.WriteLine($"🚀 API Call: POST /api/auth/signup");
            await _jsRuntime.InvokeVoidAsync("logApiCall", "POST", "/api/auth/signup", System.Text.Json.JsonSerializer.Serialize(signupData));
            
            // UI only sends simple data structure - no command creation here
            var response = await _httpClient.PostAsJsonAsync("api/auth/signup", signupData);
            
            Console.WriteLine($"📥 Response Status: {response.StatusCode}");
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                Console.WriteLine($"✅ Success Response: {System.Text.Json.JsonSerializer.Serialize(result)}");
                
                // Safe JavaScript logging
                try
                {
                    await _jsRuntime.InvokeVoidAsync("logApiResponse", (int)response.StatusCode, System.Text.Json.JsonSerializer.Serialize(result));
                }
                catch (InvalidOperationException)
                {
                    // Ignore JS interop errors during prerendering
                }
                
                if (result?.IsSuccess == true && !string.IsNullOrEmpty(result.Token))
                {
                    await _localStorage.SetItemAsync(TokenKey, result.Token);
                    if (_authStateProvider is CustomAuthStateProvider customProvider)
                    {
                        customProvider.NotifyAuthenticationStateChanged(result.User);
                    }
                }
                
                return result ?? new AuthResponse { IsSuccess = false, Message = "Invalid response from server." };
            }
            
            var errorResult = await response.Content.ReadFromJsonAsync<AuthResponse>();
            Console.WriteLine($"❌ Error Response: {System.Text.Json.JsonSerializer.Serialize(errorResult)}");
            
            // Safe JavaScript logging
            try
            {
                await _jsRuntime.InvokeVoidAsync("logApiResponse", (int)response.StatusCode, System.Text.Json.JsonSerializer.Serialize(errorResult));
            }
            catch (InvalidOperationException)
            {
                // Ignore JS interop errors during prerendering
            }
            
            return errorResult ?? new AuthResponse { IsSuccess = false, Message = "Registration failed. Please try again." };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Exception: {ex.Message}");
            
            // Safe JavaScript logging
            try
            {
                await _jsRuntime.InvokeVoidAsync("console.error", $"💥 API Exception: {ex.Message}");
            }
            catch (InvalidOperationException)
            {
                // Ignore JS interop errors during prerendering
            }
            
            return new AuthResponse { IsSuccess = false, Message = $"Network error: {ex.Message}" };
        }
    }

    /// <summary>
    /// Sends signin data to API - API creates the command internally
    /// </summary>
    public async Task<AuthResponse> SignInAsync(SigninData signinData)
    {
        try
        {
            // Log to server console only (safe during prerendering)
            Console.WriteLine($"🚀 API Call: POST /api/auth/signin");
            
            // Safe JavaScript logging (only if not prerendering)
            try
            {
                await _jsRuntime.InvokeVoidAsync("logApiCall", "POST", "/api/auth/signin", System.Text.Json.JsonSerializer.Serialize(signinData));
            }
            catch (InvalidOperationException)
            {
                // Ignore JS interop errors during prerendering
            }
            
            // UI only sends simple data structure - no command creation here
            var response = await _httpClient.PostAsJsonAsync("api/auth/signin", signinData);
            
            Console.WriteLine($"📥 Response Status: {response.StatusCode}");
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                Console.WriteLine($"✅ Success Response: {System.Text.Json.JsonSerializer.Serialize(result)}");
                
                // Safe JavaScript logging
                try
                {
                    await _jsRuntime.InvokeVoidAsync("logApiResponse", (int)response.StatusCode, System.Text.Json.JsonSerializer.Serialize(result));
                }
                catch (InvalidOperationException)
                {
                    // Ignore JS interop errors during prerendering
                }
                
                if (result?.IsSuccess == true && !string.IsNullOrEmpty(result.Token))
                {
                    await _localStorage.SetItemAsync(TokenKey, result.Token);
                    if (_authStateProvider is CustomAuthStateProvider customProvider)
                    {
                        customProvider.NotifyAuthenticationStateChanged(result.User);
                    }
                }
                
                return result ?? new AuthResponse { IsSuccess = false, Message = "Invalid response from server." };
            }
            
            var errorResult = await response.Content.ReadFromJsonAsync<AuthResponse>();
            Console.WriteLine($"❌ Error Response: {System.Text.Json.JsonSerializer.Serialize(errorResult)}");
            
            // Safe JavaScript logging
            try
            {
                await _jsRuntime.InvokeVoidAsync("logApiResponse", (int)response.StatusCode, System.Text.Json.JsonSerializer.Serialize(errorResult));
            }
            catch (InvalidOperationException)
            {
                // Ignore JS interop errors during prerendering
            }
            
            return errorResult ?? new AuthResponse { IsSuccess = false, Message = "Sign in failed. Please try again." };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Exception: {ex.Message}");
            
            // Safe JavaScript logging
            try
            {
                await _jsRuntime.InvokeVoidAsync("console.error", $"💥 API Exception: {ex.Message}");
            }
            catch (InvalidOperationException)
            {
                // Ignore JS interop errors during prerendering
            }
            
            return new AuthResponse { IsSuccess = false, Message = $"Network error: {ex.Message}" };
        }
    }
}