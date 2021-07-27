using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using ReadingIsGood.Utils.Jwt;

namespace ReadingIsGood.Api.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Registers JWT middleware for target controller using specific attributes and middleware per endpoint
        /// </summary>
        /// <typeparam name="TMiddlewareAttribute"></typeparam>
        /// <typeparam name="TMiddleware"></typeparam>
        /// <param name="app">The app builder</param>
        /// <param name="targetController">The target controller containing the specific JWT attributes</param>
        public static void RegisterJwtCheckMiddleware<TMiddlewareAttribute, TMiddleware>(this IApplicationBuilder app, Type targetController)
            where TMiddlewareAttribute : JwtCheckBaseAttribute
        {
            var actionsProtectedByJwtTempOtpToken = targetController
                .GetMethods()
                .Where(x => !x.IsConstructor
                            && x.IsPublic
                            && x.CustomAttributes.Any(y => typeof(TMiddlewareAttribute) == y.AttributeType));

            foreach (var item in actionsProtectedByJwtTempOtpToken)
            {
                var segments = item
                    .GetCustomAttributes()
                    .OfType<TMiddlewareAttribute>()
                    .FirstOrDefault()
                    ?.PathSegments;

                if (segments?.Any() != true)
                {
                    throw new Exception($"No routing configuration detected. Cannot use [{nameof(TMiddlewareAttribute)}] without route configuration!");
                }

                foreach (var segment in segments)
                {
                    app.UseWhen(context => context.Request.Path.Value.EndsWith(segment), (appBuilder) => appBuilder.UseMiddleware<TMiddleware>());
                }
            }
        }
    }
}
