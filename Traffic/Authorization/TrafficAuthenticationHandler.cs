using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Traffic.Application.Interfaces;
using Traffic.Utilities.Constants;
using Traffic.Utilities.Helpers;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Traffic.Api.Authorization
{
    public class TrafficAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IAuthorizationService _authorizationService;

        private IConfiguration _configuration { get; }

        public TrafficAuthenticationHandler(
            IAuthorizationService authorizationService,
            IConfiguration configuration,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _authorizationService = authorizationService;
            _configuration = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Path.ToString().Contains("swagger"))
            {
                return AuthenticateResult.NoResult();
            }

            var hasAuthHeader = Request.Headers.TryGetValue(CommonConstants.Authorization, out var authHeaders);

            if (!hasAuthHeader || authHeaders.Count < 1)
            {
                return AuthenticateResult.Fail("Token is invalid or it is not represented in request headers.");
            }

            var (scheme, token) = ParseAuthHeader(authHeaders[0]);

            if (!CommonConstants.BearerSchema.Equals(scheme, StringComparison.InvariantCultureIgnoreCase) &&
                !CommonConstants.BasicSchema.Equals(scheme, StringComparison.InvariantCultureIgnoreCase)
               )
            {
                return AuthenticateResult.Fail("Invalid authentication scheme.");
            }

            if (scheme == CommonConstants.BearerSchema)
            {
                return await HandleAuthenticateBearerSchemaAsync(token);
            }
            else
            {
                return await HandleAuthenticateBasicSchemaAsync(token);
            }
        }

        private static (string Scheme, string Token) ParseAuthHeader(string authHeader)
        {
            string scheme = null;
            string token = null;

            if (string.IsNullOrEmpty(authHeader))
                return (scheme, token);

            var parts = authHeader.Split(' ');
            scheme = parts[0];

            if (parts.Length > 1)
            {
                token = parts[1];
            }

            return (scheme, token);
        }

        private async Task<AuthenticateResult> HandleAuthenticateBearerSchemaAsync(string token)
        {
            // Get user profile
            var user = await _authorizationService.GetUserAsync(token);

            if (user == null || user.Id == null)
            {
                return AuthenticateResult.Fail("Invalid token");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimConstants.Permission, string.Join(";","user.Permissions"))
                
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return await Task.Run(() => AuthenticateResult.Success(ticket));
        }

        private async Task<AuthenticateResult> HandleAuthenticateBasicSchemaAsync(string token)
        {
            string username;
            string password;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers[CommonConstants.Authorization]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                username = credentials[0];
                password = credentials[1];
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (!((username == _configuration["Traffic:BasicAuthen:UserName"]) && (password == Cryptography.DecryptString(_configuration["Traffic:BasicAuthen:Password"]))))
            {
                return AuthenticateResult.Fail("Invalid Username or Password");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "basicUsername"),
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return await Task.Run(() => AuthenticateResult.Success(ticket));
        }
    }
}
