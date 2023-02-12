using System.Security.Claims;

namespace FoodRatingApi.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new Exception("Claims principal does not contain name identifier claim.");
    }
}
