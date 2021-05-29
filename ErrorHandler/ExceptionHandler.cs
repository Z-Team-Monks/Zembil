using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Zembil.ErrorHandler
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment en;
        public ExceptionHandler(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            en = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {

                await _next(context);
            }
            catch (Exception err)
            {
                await HandleCustomException(context, err);
            }
        }

        public async Task HandleCustomException(HttpContext context, Exception err)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            string StatusMessage = "error";
            string ErrorMessage = "Internal Server Error";
            switch (err)
            {
                case CustomAppException e:
                    CustomAppException ex = (CustomAppException)err;
                    response.StatusCode = ex.errorDetail.StatusCode;
                    StatusMessage = ex.errorDetail.Status;
                    ErrorMessage = ex.errorDetail.Message;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            if (en.EnvironmentName == "Development")
            {
                Console.WriteLine(err.Message);
                ErrorMessage = err.Message;
            }
            await response.WriteAsync(new ErrorDetail
            {
                StatusCode = response.StatusCode,
                Status = StatusMessage,
                Message = ErrorMessage
            }.ToString());
        }
    }
}