using System.Security.Claims;

namespace api.Extensions;

public static class ClaimsExtensions
{
    public static string? GetUsername(this ClaimsPrincipal user)
    {
        var claims = user.Claims.ToList();
        foreach (var c in claims)
        {
            Console.WriteLine($"Claim Type: {c.Type}, Claim Value: {c.Value}");
        }

        var claim = claims.SingleOrDefault(x =>
            x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
        );
        return claim?.Value;
    }
}
