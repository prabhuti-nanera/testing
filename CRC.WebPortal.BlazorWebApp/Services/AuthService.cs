using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using CRC.WebPortal.BlazorWebApp.Models;

namespace CRC.WebPortal.BlazorWebApp.Services;

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


    public async Task<AuthResponse> SignUpAsync(SignupData signupData)
    {
        return await ExecuteAuthRequestAsync("api/auth/signup", signupData, "Registration failed. Please try again.");
    }

    public async Task<AuthResponse> SignInAsync(SigninData signinData)
    {
        return await ExecuteAuthRequestAsync("api/auth/signin", signinData, "Sign in failed. Please try again.");
    }

    private async Task<AuthResponse> ExecuteAuthRequestAsync<T>(string endpoint, T data, string defaultErrorMessage)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                
                if (result?.IsSuccess == true && !string.IsNullOrEmpty(result.Token))
                {
                    await _localStorage.SetItemAsync(TokenKey, result.Token);
                    if (_authStateProvider is CustomAuthStateProvider customProvider && result.User != null)
                    {
                        customProvider.NotifyAuthenticationStateChanged(result.User);
                    }
                }
                
                return result ?? new AuthResponse { IsSuccess = false, Message = "Invalid response from server." };
            }
            
            var errorResult = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return errorResult ?? new AuthResponse { IsSuccess = false, Message = defaultErrorMessage };
        }
        catch (Exception ex)
        {
            return new AuthResponse { IsSuccess = false, Message = $"Network error: {ex.Message}" };
        }
    }


    public async Task LogoutAsync()
    {
        try
        {
            // Clear the token from local storage
            await _localStorage.RemoveItemAsync(TokenKey);
            
            // Notify auth state provider that user is logged out
            ((CustomAuthStateProvider)_authStateProvider).NotifyAuthenticationStateChanged(null!);
        }
        catch (Exception)
        {
            // Silent error handling for logout
        }
    }

}