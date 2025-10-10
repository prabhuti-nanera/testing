using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace CRC.WebPortal.BlazorUI.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;
    private const string TokenKey = "authToken";

    public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>(TokenKey);

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var claims = ParseClaimsFromJwt(token);
            var expiry = claims.FirstOrDefault(x => x.Type.Equals("exp"))?.Value;

            if (expiry != null)
            {
                var datetime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiry));
                if (datetime.UtcDateTime <= DateTime.UtcNow)
                {
                    await _localStorage.RemoveItemAsync(TokenKey);
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            return new AuthenticationState(user);
        }
        catch
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public void NotifyUserAuthentication(string token)
    {
        var claims = ParseClaimsFromJwt(token);
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        NotifyAuthenticationStateChanged(authState);
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        return token.Claims;
    }
}
