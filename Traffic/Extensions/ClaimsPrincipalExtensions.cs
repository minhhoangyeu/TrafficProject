using Traffic.Utilities.Constants;
using System.Linq;
using System.Security.Claims;

namespace Traffic.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity).Claims.SingleOrDefault(x => x.Type == ClaimConstants.UserId);
            return (claim != null) ? claim.Value : null;
        }

        public static string GetSpecificClaim(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == claimType);
            return (claim != null) ? claim.Value : null;
        }
    }
}
