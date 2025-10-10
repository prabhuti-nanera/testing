using System.Net.Http.Json;
using Blazored.LocalStorage;
using CRC.WebPortal.BlazorUI.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace CRC.WebPortal.BlazorUI.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;
    private const string TokenKey = "authToken";
    private const string RefreshTokenKey = "refreshToken";

    public AuthService(
        HttpClient httpClient,
        ILocalStorageService localStorage,
        AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<UserDto> SignInAsync(string email, string password)
    {
        var command = new SignInCommand { Email = email, Password = password };
        var response = await _httpClient.PostAsJsonAsync("api/auth/signin", command);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (result != null && !string.IsNullOrEmpty(result.Token))
        {
            await _localStorage.SetItemAsync(TokenKey, result.Token);
            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                await _localStorage.SetItemAsync(RefreshTokenKey, result.RefreshToken);
            }

            ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);

            return result.User ?? new UserDto();
        }

        throw new Exception("Authentication failed");
    }

    public async Task<UserDto> SignUpAsync(string email, string password, string firstName, string lastName)
    {
        var command = new SignUpCommand 
        { 
            Email = email, 
            Password = password, 
            FirstName = firstName, 
            LastName = lastName 
        };
        
        var response = await _httpClient.PostAsJsonAsync("api/auth/signup", command);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (result != null && !string.IsNullOrEmpty(result.Token))
        {
            await _localStorage.SetItemAsync(TokenKey, result.Token);
            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                await _localStorage.SetItemAsync(RefreshTokenKey, result.RefreshToken);
            }

            ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);

            return result.User ?? new UserDto();
        }

        throw new Exception("Registration failed");
    }

    public async Task<AuthResponse> RefreshTokenAsync()
    {
        var refreshToken = await _localStorage.GetItemAsync<string>(RefreshTokenKey);
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new Exception("No refresh token available");
        }

        var response = await _httpClient.PostAsJsonAsync("api/auth/refresh", new { RefreshToken = refreshToken });
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (result != null && !string.IsNullOrEmpty(result.Token))
        {
            await _localStorage.SetItemAsync(TokenKey, result.Token);
            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                await _localStorage.SetItemAsync(RefreshTokenKey, result.RefreshToken);
            }

            ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
        }

        return result ?? new AuthResponse();
    }

    public async Task<AuthResponse> ForgotPasswordAsync(object request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/forgot-password", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AuthResponse>() ?? new AuthResponse();
    }

    public async Task<AuthResponse> ResetPasswordAsync(object request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/reset-password", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AuthResponse>() ?? new AuthResponse();
    }

    public async Task SignOutAsync()
    {
        await _localStorage.RemoveItemAsync(TokenKey);
        await _localStorage.RemoveItemAsync(RefreshTokenKey);
        ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
    }

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        var token = await _localStorage.GetItemAsync<string>(TokenKey);
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        try
        {
            var response = await _httpClient.GetAsync("api/auth/me");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserDto>();
            }
        }
        catch
        {
            // Token might be expired
        }

        return null;
    }
}
