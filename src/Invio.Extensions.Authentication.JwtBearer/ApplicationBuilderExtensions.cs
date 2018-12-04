using System;
using Microsoft.AspNetCore.Builder;

namespace Invio.Extensions.Authentication.JwtBearer {

    /// <summary>
    ///   Eases the application of the additonal middlewares necessary to fulfill
    ///   the <see cref="IJwtBearerQueryStringBehavior" /> defined on the web service's
    ///   <see cref="JwtBearerQueryStringOptions" />.
    /// </summary>
    public static class ApplicationBuilderExtensions {

        /// <summary>
        ///   Places the <see cref="JwtBearerQueryStringMiddleware" /> into the request
        ///   pipeline, ensuring the <see cref="IJwtBearerQueryStringBehavior" />
        ///   defined gets applied.
        /// </summary>
        /// <remarks>
        ///   This should be done after authentication middleware, but before any logging
        ///   middleware. That will ensure the access token is used to authenticate the user,
        ///   but nothing else gets to see it unless the behavior was configured to allow it.
        /// </remarks>
        public static IApplicationBuilder UseJwtBearerQueryString(
            this IApplicationBuilder builder) {

            if (builder == null) {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.UseMiddleware<JwtBearerQueryStringMiddleware>();
        }

    }

}