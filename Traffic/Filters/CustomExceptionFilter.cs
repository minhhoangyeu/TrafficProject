using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Traffic.Api.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            int? statusCode;

            if(context.Exception is ArgumentException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (context.Exception is InvalidOperationException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
            }

            // Customize this object to fit your needs
            var result = new ObjectResult(new
            {
                Message = context.Exception.Message,
                ExceptionType = context.Exception.GetType().FullName,
                StatusCode = statusCode
            });

            // Log the exception
            _logger.LogError("Unhandled exception occurred while executing request: {ex}", context.Exception);

            // Set the result
            context.Result = result;
        }
    }
}
