using System;
using Microsoft.AspNetCore.Authentication;

namespace Invio.Extensions.Authentication.JwtBearer {

    /// <summary>
    ///   Eases the application of the additonal behaviors to a given JWT bearer
    ///   configuration via its <see cref="AuthenticationBuilder" />.
    /// </summary>
    public static class AuthenticationBuilderExtensions {

        /// <summary>
        ///   Adds authentication of bearer tokens via URI Query Parameters as defined in
        ///   <see href="https://tools.ietf.org/html/rfc6750#section-2.3">RFC 6750</see>.
        ///   The name of the query string parameter sought will be the default,
        ///   "access_token", and the parameter will be redacted via the included
        ///   <see cref="JwtBearerQueryStringMiddleware" />.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="AuthenticationBuilder" /> object being used to construct
        ///   the configuration for a given JWT bearer authentication handler.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="builder" /> is null.
        /// </exception>
        public static AuthenticationBuilder AddJwtBearerQueryStringAuthentication(
            this AuthenticationBuilder builder) {

            return builder.AddJwtBearerQueryStringAuthentication(_ => {});
        }

        /// <summary>
        ///   Adds authentication of bearer tokens via URI Query Parameters as defined in
        ///   <see href="https://tools.ietf.org/html/rfc6750#section-2.3">RFC 6750</see>.
        ///   The name of the query string parameter sought will be the default,
        ///   "access_token", and the parameter will be redacted via the included
        ///   <see cref="JwtBearerQueryStringMiddleware" />, unless otherwise configured
        ///   via the <paramref name="configureOptions" /> parameter.
        /// </summary>
        /// <param name="builder">
        ///   The <see cref="AuthenticationBuilder" /> object being used to construct
        ///   the configuration for a given JWT bearer authentication handler.
        /// </param>
        /// <param name="configureOptions">
        ///   An action used to configure how the JWT bearer token should be handled
        ///   if it is being passed in via the query string.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="builder" /> or
        ///   <paramref name="configureOptions" /> is null.
        /// </exception>
        public static AuthenticationBuilder AddJwtBearerQueryStringAuthentication(
            this AuthenticationBuilder builder,
            Action<JwtBearerQueryStringOptions> configureOptions) {

            if (builder == null) {
                throw new ArgumentNullException(nameof(builder));
            } else if (configureOptions == null) {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            builder.Services.AddJwtBearerQueryStringAuthentication(configureOptions);

            return builder;
        }

    }

}