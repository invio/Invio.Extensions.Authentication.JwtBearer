using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Invio.Extensions.Authentication.JwtBearer {

    /// <summary>
    ///   Eases the application of the additonal behaviors to a given JWT bearer
    ///   configuration via its <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions {

        /// <summary>
        ///   Adds authentication of bearer tokens via URI Query Parameters as defined in
        ///   <see href="https://tools.ietf.org/html/rfc6750#section-2.3">RFC 6750</see>.
        ///   The name of the query string parameter sought will be the default,
        ///   "access_token", and the parameter will be redacted via the included
        ///   <see cref="JwtBearerQueryStringMiddleware" />.
        /// </summary>
        /// <param name="services">
        ///   The <see cref="IServiceCollection" /> object being used to construct
        ///   the configuration for a given JWT bearer authentication handler.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="services" /> is null.
        /// </exception>
        public static IServiceCollection AddJwtBearerQueryStringAuthentication(
            this IServiceCollection services) {

            return services.AddJwtBearerQueryStringAuthentication(_ => {});
        }

        /// <summary>
        ///   Adds authentication of bearer tokens via URI Query Parameters as defined in
        ///   <see href="https://tools.ietf.org/html/rfc6750#section-2.3">RFC 6750</see>.
        ///   The name of the query string parameter sought will be the default,
        ///   "access_token", and the parameter will be redacted via the included
        ///   <see cref="JwtBearerQueryStringMiddleware" />, unless otherwise configured
        ///   via the <paramref name="configureOptions" /> parameter.
        /// </summary>
        /// <param name="services">
        ///   The <see cref="IServiceCollection" /> object being used to construct
        ///   the configuration for a given JWT bearer authentication handler.
        /// </param>
        /// <param name="configureOptions">
        ///   An action used to configure how the JWT bearer token should be handled
        ///   if it is being passed in via the query string.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="services" /> or
        ///   <paramref name="configureOptions" /> is null.
        /// </exception>
        public static IServiceCollection AddJwtBearerQueryStringAuthentication(
            this IServiceCollection services,
            Action<JwtBearerQueryStringOptions> configureOptions) {

            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            } else if (configureOptions == null) {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.Configure<JwtBearerQueryStringOptions>(configureOptions);
            
            services.AddSingleton<
                IPostConfigureOptions<JwtBearerOptions>,
                JwtBearerOptionsPostConfiguration>();

            return services;
        }

    }

}