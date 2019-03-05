using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CustomMiddleWare
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MathMiddleware : ICalculator
    {
        private readonly RequestDelegate _next;

        public MathMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            return _next(httpContext);
        }

        public double? ExecuteOperation(string operatorName, string[] arguments)
        {
            return null;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MathMiddlewareExtensions
    {
        public static IApplicationBuilder UseMath(this IApplicationBuilder builder)
        {
            builder.Use(next =>
            {
                return async context =>
                {
                    if (context.Request.Path.StartsWithSegments("/math", out var operatorPath))
                    {
                        var reader = new StreamReader(context.Request.Body);
                        var body = reader.ReadToEndAsync();
                        var args = body.Result.Split(new char[] {',', ' ', '-'}, StringSplitOptions.RemoveEmptyEntries);

                                                switch (operatorPath)
                        {
                            case "/add":
                                await context.Response.WriteAsync("Result: " + Add(args));
                                break;
                            case "/subtract":
                                await context.Response.WriteAsync("Result: " + Subtract(args));
                                break;
                            case "/factorial":
                                await context.Response.WriteAsync("Result: " + Factorial(args));
                                break;
                            default:
                                await next(context);
                                break;
                        }
                        //await context.Response.WriteAsync("Hit! " + operatorPath);
                    }
                    else
                    {
                        await next(context);
                    }
                };
            });

            return builder.UseMiddleware<MathMiddleware>();
        }

        private static string Factorial(string[] args)
        {
            if (args.Length != 1)
            {
                return null;
            }
            else
            {
                int arg = Convert.ToInt32(args[0]);
                int result = 1;
                for (int i = 1; i <= arg; i++)
                {
                    result = result * i;
                }

                return result.ToString();
            }
        }

        private static string Subtract(string[] args)
        {
            if (args.Length == 0)
            {
                return null;
            }
            else
            {
                double result = Convert.ToDouble(args[0]);

                for (int i = 1; i < args.Length; i++)
                {
                    result = result - Convert.ToDouble(args[i]);
                }

                return result.ToString();
            }
        }

        private static string Add(string[] args)
        {
            double result = 0;

            for (int i = 0; i < args.Length; i++)
            {
                result += Convert.ToDouble(args[i]);
            }

            return result.ToString();
        }
    }
}
