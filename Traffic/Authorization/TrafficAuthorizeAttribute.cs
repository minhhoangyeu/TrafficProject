using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Traffic.Utilities.Constants;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Traffic.Api.Authorization
{
    public class TrafficAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public string[] Permissions { get; private set; }

        public TrafficAuthorizeAttribute(params string[] permissions) : base()
        {
            Permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
        }

        public virtual async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var isAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
            if (!isAuthenticated)
            {
                var unauthorizedResult = new ObjectResult(new
                {
                    Message = "Access Denied",
                    StatusCode = HttpStatusCode.Unauthorized
                });

                context.Result = unauthorizedResult;
                return;
            }

            var forbidResult = new ObjectResult(new
            {
                Message = "You do not have permission to perform this action",
                StatusCode = HttpStatusCode.Forbidden
            });

            var permissionClaim = context.HttpContext.User.FindFirst(c => c.Type == ClaimConstants.Permission)?.Value;

            if (string.IsNullOrEmpty(permissionClaim))
            {
                context.Result = forbidResult;
                return;
            }

            var permissionClaimList = permissionClaim.Split(';').ToList();
            foreach (var permission in Permissions)
            {
                if (permissionClaimList.Any(p => string.Equals(p, permission, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return;
                }
            }

            context.Result = forbidResult;
            await Task.CompletedTask;
        }
    }
}
