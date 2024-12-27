using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text.Json;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ClaimsPrincipal _user;

    public CustomAuthenticationStateProvider(ILocalStorageService localStorageService, IHttpContextAccessor httpContextAccessor)
    {
        _localStorageService = localStorageService;
        _httpContextAccessor = httpContextAccessor;
        _user = new ClaimsPrincipal(new ClaimsIdentity());
    }

    public async Task InitializeAsync()
    {
        var token = await _localStorageService.GetItemAsync<string>("authToken");
        if (!string.IsNullOrEmpty(token))
        {
            await MarkUserAsAuthenticated(token);
        }
        else
        {
            MarkUserAsLoggedOut();
        }
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_httpContextAccessor?.HttpContext == null)
        {
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }

        var user = _httpContextAccessor.HttpContext.User ?? new ClaimsPrincipal(new ClaimsIdentity());
        return Task.FromResult(new AuthenticationState(user));
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        try
        {
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            _user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error marking user as authenticated: {ex.Message}");
            MarkUserAsLoggedOut();
        }
    }

    public void MarkUserAsLoggedOut()
    {
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        if (string.IsNullOrEmpty(jwt) || jwt.Split('.').Length != 3)
        {
            throw new ArgumentException("Invalid JWT token");
        }

        try
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = WebEncoders.Base64UrlDecode(payload);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            if (keyValuePairs == null)
            {
                throw new InvalidOperationException("Failed to parse claims from JWT.");
            }

            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? string.Empty));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing JWT: {ex.Message}");
            return Enumerable.Empty<Claim>();
        }
    }
}
