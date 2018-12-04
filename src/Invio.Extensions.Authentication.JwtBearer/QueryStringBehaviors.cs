using System;

namespace Invio.Extensions.Authentication.JwtBearer {

    /// <summary>
    ///   Various methods and properties that provide an idiomatic way to define
    ///   the desired <see cref="IJwtBearerQueryStringBehavior" /> for a given
    ///   instance of <see cref="JwtBearerQueryStringOptions" />.
    /// </summary>
    public static class QueryStringBehaviors {

        /// <summary>
        ///   This behavior will cause the access token to be redacted from the
        ///   user's web request by replacing it with the string "(REDACTED)".
        ///   This is the default behavior.
        /// </summary>
        public static IJwtBearerQueryStringBehavior Redact { get; }

        /// <summary>
        ///   This behavior will perform no mutations to the user's web request.
        /// </summary>
        /// <remarks>
        ///   This behavior is not recommended for production environments.
        ///   However, it may be useful in development and test scenarios.
        /// </remarks>
        public static IJwtBearerQueryStringBehavior None { get; }

        static QueryStringBehaviors() {
            Redact = new RedactJwtBearerQueryStringBehavior();
            None = new NullJwtBearerQueryStringBehavior();
        }

        /// <summary>
        ///   This behavior will cause the access token to be redacted from the
        ///   user's web request by replacing it with the string provided via
        ///   the <paramref name="redactedValue" />.
        /// </summary>
        /// <param name="redactedValue">
        ///   The string that will be put in the place of the access token if
        ///   it is found in the query string.
        /// </param>
        public static IJwtBearerQueryStringBehavior RedactWithValue(string redactedValue) {
            return new RedactJwtBearerQueryStringBehavior(redactedValue);
        }

    }

}