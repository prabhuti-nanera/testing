using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using CRC.WebPortal.Application.Common.Models;
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
        try
        {
            // UI only sends simple data structure - no command creation here
            var response = await _httpClient.PostAsJsonAsync("api/auth/signup", signupData);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                
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
            return errorResult ?? new AuthResponse { IsSuccess = false, Message = "Registration failed. Please try again." };
        }
        catch (Exception ex)
        {
            return new AuthResponse { IsSuccess = false, Message = $"An error occurred: {ex.Message}" };
        }
    }

    public async Task<AuthResponse> SignInAsync(SigninData signinData)
    {
        try
        {
            // UI only sends simple data structure - no command creation here
            var response = await _httpClient.PostAsJsonAsync("api/auth/signin", signinData);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                
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
            return errorResult ?? new AuthResponse { IsSuccess = false, Message = "Sign in failed. Please try again." };
        }
        catch (Exception ex)
        {
            return new AuthResponse { IsSuccess = false, Message = $"Network error: {ex.Message}" };
        }
    }

    public async Task<AuthResponse> ForgotPasswordAsync(ForgotPasswordData forgotPasswordData)
    {
        try
        {
            // Create simple request object for API
            var request = new ForgotPasswordDataRequest
            {
                Email = forgotPasswordData.Email
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/forgot-password", request);
            
            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                return authResponse ?? new AuthResponse { IsSuccess = false, Message = "Invalid response from server" };
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                return errorResponse ?? new AuthResponse { IsSuccess = false, Message = "Failed to process forgot password request" };
            }
        }
        catch (Exception ex)
        {
            return new AuthResponse { IsSuccess = false, Message = $"Network error: {ex.Message}" };
        }
    }

    public async Task<AuthResponse> ResetPasswordAsync(ResetPasswordData resetPasswordData)
    {
        try
        {
            // Create simple request object for API
            var request = new ResetPasswordDataRequest
            {
                Email = resetPasswordData.Email,
                ResetCode = resetPasswordData.ResetCode,
                NewPassword = resetPasswordData.NewPassword
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/reset-password", request);
            
            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                return authResponse ?? new AuthResponse { IsSuccess = false, Message = "Invalid response from server" };
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                return errorResponse ?? new AuthResponse { IsSuccess = false, Message = "Failed to reset password" };
            }
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
        catch (Exception ex)
        {
            // Silent error handling for logout
        }
    }

    private class ForgotPasswordDataRequest
    {
        public string Email { get; set; } = string.Empty;
    }

    private class ResetPasswordDataRequest
    {
        public string Email { get; set; } = string.Empty;
        public string ResetCode { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}