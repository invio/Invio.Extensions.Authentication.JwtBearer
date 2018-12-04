using System;
using Microsoft.AspNetCore.Http;

namespace Invio.Extensions.Authentication.JwtBearer {

    /// <summary>
    ///   A base implementation of the <see cref="IJwtBearerQueryStringBehavior" /> that
    ///   validates parameters to conform with the interface's requirements that cannot
    ///   be enforced in the compiler.
    /// </summary>
    public abstract class JwtBearerQueryStringBehaviorBase : IJwtBearerQueryStringBehavior {

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
        public void Apply(HttpContext context, JwtBearerQueryStringOptions options) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            } else if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            this.ApplyImpl(context, options);
        }

        /// <summary>
        ///   This is the same behavior as the <see cref="Apply" /> method sans the boilerplace
        ///   <see cref="ArgumentNullException" /> checks.
        /// </summary>
        protected abstract void ApplyImpl(
            HttpContext context,
            JwtBearerQueryStringOptions options);

    }

}