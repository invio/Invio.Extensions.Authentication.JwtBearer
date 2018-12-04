using System;

namespace Invio.Extensions.Authentication.JwtBearer {

    /// <summary>
    ///   This class defines the configuration of how to query string parameter-based
    ///   authentication should behave in the service that is consuming this library.
    /// </summary>
    public sealed class JwtBearerQueryStringOptions {

        /// <summary>
        ///   The default query string parameter name sought in the web request's URI.
        /// </summary>
        /// <remarks>
        ///   The name 'access_token' was chosen as it is listed in RFC 6750.
        /// </remarks>
        public static string DefaultQueryStringParameterName { get; } = "access_token";

        /// <summary>
        ///   The name of the query string parameter that will be sought in
        ///   the web request's URI.
        /// </summary>
        public string QueryStringParameterName { get; set; } = DefaultQueryStringParameterName;

        /// <summary>
        ///   This defines how the query string should be managed in the
        ///   <see cref="JwtBearerQueryStringMiddleware" />. For example, it may
        ///   redact the token and inject a placeholder string in its place, or
        ///   it make leave the token alone entirely.
        /// </summary>
        /// <remarks>
        ///   By default, we redact the query string by swapping its value will be
        ///   replaced with the word "(REDACTED)". This still lets you know that the
        ///   user did authenticate via the query string, but token used for
        ///   authentication is being hidden due to security concerns.
        /// </remarks>
        public IJwtBearerQueryStringBehavior QueryStringBehavior { get; set; } =
            QueryStringBehaviors.Redact;

    }

}