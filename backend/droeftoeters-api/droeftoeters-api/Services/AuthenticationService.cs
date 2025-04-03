using System.Security.Claims;
using droeftoeters_api.Interfaces;

namespace droeftoeters_api.Services;

/// <summary>
/// Based on the example code provided by Microsoft
/// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-context?view=aspnetcore-9.0&preserve-view=true
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetCurrentUserId()
    {
        //Returns userid of the current user
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}