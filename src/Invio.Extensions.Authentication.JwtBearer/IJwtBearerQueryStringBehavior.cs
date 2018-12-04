using System;
using Microsoft.AspNetCore.Http;

namespace Invio.Extensions.Authentication.JwtBearer {

    /// <summary>
    ///   Defines how the web service should mutate requests, if at all, in order
    ///   to manage the query string parameter's
    /// </summary>
    public interface IJwtBearerQueryStringBehavior {

        /// <summary>
        ///   Applies the defined query string mutation behavior to the provided
        ///   <see cref="HttpContext" />.
        /// </summary>
        /// <remarks>
        ///   Defined in the <see cref="JwtBearerQueryStringOptions" />, this may
        ///   do something like redact the token, it may move the token into the
        ///   'Authorization' header using the traditional Bearer authentication
        ///   scheme, or it may do nothing at all.
        /// </remarks>
        /// <param name="context">
        ///   The <see cref="HttpContext" /> for a given request. The request may or may
        ///   not be using the query string as a form of authentication.
        /// </param>
        /// <param name="options">
        ///   The <see cref="JwtBearerQueryStringOptions" /> that define how the
        ///   query string authentication should be managed within this web service.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="context" /> or <paramref name="options" /> is null.
        /// </exception>
        void Apply(HttpContext context, JwtBearerQueryStringOptions options);

    }

}