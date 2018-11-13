using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Invio.Extensions.Authentication.JwtBearer {

    /// <summary>
    ///   Eases the application of the additonal behaviors to a given JWT bearer
    ///   configuration via its <see cref="JwtBearerOptions" />.
    /// </summary>
    public static class JwtBearerOperationsExtensions {

        /// <summary>
        ///   Adds authentication of bearer tokens via URI Query Parameters as defined in
        ///   <see href="https://tools.ietf.org/html/rfc6750#section-2.3">RFC 6750</see>.
        ///   The name of the query string parameter sought will be the default, "access_token".
        /// </summary>
        /// <param name="options">
        ///   A <see cref="JwtBearerOptions" /> object that describes
        ///   the current configuration for the authentication of bearer tokens.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="options" /> is null.
        /// </exception>
        public static JwtBearerOptions AddQueryStringAuthentication(
            this JwtBearerOptions options) {

            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            options.Events =
                new QueryStringJwtBearerEventsWrapper(
                    options.Events ?? new JwtBearerEvents()
                );

            return options;
        }

        /// <summary>
        ///   Adds authentication of bearer tokens via URI Query Parameters as defined in
        ///   <see href="https://tools.ietf.org/html/rfc6750#section-2.3">RFC 6750</see>.
        ///   The name of the query string parameter sought will be that which is passed
        ///   in to the <paramref name="queryStringParameterName" /> parameter.
        /// </summary>
        /// <param name="options">
        ///   A <see cref="JwtBearerOptions" /> object that describes
        ///   the current configuration for the authentication of bearer tokens.
        /// </param>
        /// <param name="queryStringParameterName">
        ///   The name of the query string parameter that will be sought in
        ///   the web request's URI.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="options" /> is null.
        /// </exception>
        public static JwtBearerOptions AddQueryStringAuthentication(
            this JwtBearerOptions options,
            string queryStringParameterName) {

            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            options.Events =
                new QueryStringJwtBearerEventsWrapper(
                    options.Events ?? new JwtBearerEvents(),
                    queryStringParameterName
                );

            return options;
        }

    }

}