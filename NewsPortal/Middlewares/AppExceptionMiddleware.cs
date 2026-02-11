using Core.Constants;
using Core.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Portal.Middlewares
{
    public static class AppExceptionMiddleware
    {
        public static IApplicationBuilder UseAppExceptionHandler(this IApplicationBuilder builder)
        {
            builder.UseExceptionHandler(conf =>
            {
                conf.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    object? exceptionObject = null;
                    if (exceptionHandlerPathFeature?.Error is AppNotFoundException error)
                    {
                        context.Response.StatusCode = 404;
                        exceptionObject = new { message = error.Message, value = error.Value };
                    }

                    if (exceptionHandlerPathFeature?.Error is AppBadDataException badDataException)
                    {
                        context.Response.StatusCode = 400;
                        exceptionObject = new { message = badDataException.Message, errors = badDataException.Errors };
                    }

                    if (exceptionHandlerPathFeature?.Error is AppConflictException conflictException)
                    {
                        context.Response.StatusCode = 409;
                        exceptionObject = new { message = conflictException.Message, value = conflictException.Value };
                    }

                    if (exceptionHandlerPathFeature?.Error is DbUpdateConcurrencyException)
                    {
                        context.Response.StatusCode = 409;
                        exceptionObject = new { message = ErrorMessages.ConcurrentException };
                    }

                    if (exceptionHandlerPathFeature?.Error is DbUpdateException)
                    {
                        context.Response.StatusCode = 409;
                        exceptionObject = new { message = ErrorMessages.DataIsInUse };
                    }

                    if (exceptionHandlerPathFeature?.Error is ValidationException validationException)
                    {
                        var errors = new Dictionary<string, string[]>();
                        foreach (var failure in validationException.Errors)
                        {
                            errors.Add(failure.PropertyName, new[] { failure.ErrorMessage });
                        }

                        context.Response.StatusCode = 400;
                        exceptionObject = new { message = "errors.validation-failed", errors };
                    }

                    if (exceptionObject == null)
                    {
                        throw exceptionHandlerPathFeature?.Error!;
                    }

                    if (context.Response != null)
                    {
                        context.Response.ContentType = "application/json";
                    }
                    var text = JsonConvert.SerializeObject(exceptionObject);

                    await context.Response!.WriteAsync($"{text}\r\n");
                    await context.Response!.WriteAsync(new string(' ', 512));
                });
            });

            return builder;
        }
    }
}
