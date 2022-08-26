using Microsoft.Extensions.Configuration;
using Traffic.Application.Interfaces;
using System;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Traffic.Application.Models.Common;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using Traffic.Utilities.Helpers;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Traffic.Api.Authorization
{
    public class TrafficAuthenticationHandler : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(token))
            {
                var responseAPI = new ApiErrorResult<string>("Lỗi xác thực");
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new JsonResult(responseAPI);
            }
            else
            {
                try
                {
                    string tokenValue = token.Replace("Bearer", string.Empty).Trim();
                    ClaimsPrincipal claimsPrincipal = DecodeJWTToken(tokenValue, EnviromentConfig.SecretKey);
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                }
                catch (SecurityTokenExpiredException ex)
                {
                    var responseAPI = new ApiErrorResult<string>("Hết phiên đăng nhập");
                    context.HttpContext.Response.StatusCode =  (int)HttpStatusCode.NotAcceptable;
                    context.Result = new JsonResult(responseAPI);
                }
                catch (Exception ex)
                {
                    var responseAPI = new ApiErrorResult<string>("Lỗi xác thực");
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new JsonResult(responseAPI);
                }
            }
        }

        public static string GetCurrentUser(IHttpContextAccessor context, string claimType)
        {
            try
            {
                string token = context.HttpContext.Request.Headers["Authorization"].ToString();
                string tokenValue = token.Replace("Bearer", string.Empty).Trim();
                ClaimsPrincipal claimsPrincipal = DecodeJWTToken(tokenValue, EnviromentConfig.SecretKey);
                string userSession = claimsPrincipal.FindFirstValue(claimType);
                return userSession;
            }
            catch
            {
                throw new ArgumentException("Lỗi xác thực");
            }
        }

        public static ClaimsPrincipal DecodeJWTToken(string token, string secretAuthKey)
        {
            var key = Encoding.ASCII.GetBytes(secretAuthKey);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(token, validations, out var tokenSecure);
            return claims;
        }
    }
    public class SystemAuthorizeAttribute : TypeFilterAttribute
    {
        public SystemAuthorizeAttribute() : base(typeof(TrafficAuthenticationHandler))
        {
        }
    }
}
