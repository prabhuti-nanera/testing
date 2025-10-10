using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.Application.Features.Auth.Commands.SignUp;
using CRC.WebPortal.BlazorUI.Models;

namespace CRC.WebPortal.BlazorUI.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;
    private const string TokenKey = "authToken";

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<UserDto> SignUpAsync(SignUpRequest request)
    {
        var command = new SignUpCommand
        {
            Email = request.Email,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var response = await _httpClient.PostAsJsonAsync("api/auth/signup", command);
        var result = await response.Content.ReadFromJsonAsync<UserDto>();

        if (result?.IsSuccess == true)
        {
            await _localStorage.SetItemAsync(TokenKey, result.Token);
            if (_authStateProvider is CustomAuthStateProvider customProvider)
            {
                customProvider.NotifyAuthenticationStateChanged(result.User);
            }
            return new UserDto { IsSuccess = true, Message = "Registration successful! Welcome aboard." };
        }

        return new UserDto { IsSuccess = false, Message = "Email already exists. Please try a different email." };
    }
}