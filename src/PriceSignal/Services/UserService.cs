using System.Security.Authentication;
using System.Security.Claims;
using Application.Common.Interfaces;

namespace PriceSignal.Services;

public class UserService : IUser
{
    public string UserIdentifier { get; } = string.Empty;
    public string Name { get; } = string.Empty;
    public string Email { get; } = string.Empty;
    public string Role { get; } = string.Empty;
    public bool IsAuthenticated { get; }
    
    public UserService() {}
    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor == null) throw new ArgumentNullException(nameof(httpContextAccessor));

        try
        {
            var httpContext = httpContextAccessor.HttpContext;
            var user = httpContext?.User;

            if (user?.Identity is not ClaimsIdentity claimsIdentity)
            {
                IsAuthenticated = false;
                return;
            }
            
            IsAuthenticated = user.Identity.IsAuthenticated;
            Name = claimsIdentity.Name ?? string.Empty;
            Email = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
            UserIdentifier = GetIdentifier(claimsIdentity);
            Role = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;
        }
        catch (Exception ex)
        {
            IsAuthenticated = false;
            throw new AuthenticationException("Not Authorised", ex);
        }
    }
    
    private string GetIdentifier(ClaimsIdentity claimsIdentity)
    {
        var userIdentifier = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        //userIdentifier = userIdentifier?.Split('|').Last();
        return userIdentifier;
    }
}