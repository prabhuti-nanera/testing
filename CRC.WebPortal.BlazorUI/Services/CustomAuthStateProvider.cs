using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using CRC.WebPortal.Application.Common.Models;

namespace CRC.WebPortal.BlazorUI.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private const string TokenKey = "authToken";
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>(TokenKey);
        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(_anonymous);
        }

        // Parse token and create claims (simplified)
        var claims = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "User") // Replace with actual claims from token
        }, "jwt");

        return new AuthenticationState(new ClaimsPrincipal(claims));
    }

    public void NotifyAuthenticationStateChanged(UserDto? user)
    {
        var authState = Task.FromResult(user == null ?
            new AuthenticationState(_anonymous) :
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName)
            }, "jwt"))));
        NotifyAuthenticationStateChanged(authState);
    }
}