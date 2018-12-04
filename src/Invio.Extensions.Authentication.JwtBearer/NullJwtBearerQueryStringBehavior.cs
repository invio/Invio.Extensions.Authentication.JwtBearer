using System;
using Microsoft.AspNetCore.Http;

namespace Invio.Extensions.Authentication.JwtBearer {

    /// <summary>
    ///   An implementation of the <see cref="IJwtBearerQueryStringBehavior" /> that
    ///   performs no mutations to the <see cref="HttpContext" /> of the user's web request.
    /// </summary>
    /// <remarks>
    ///   This can be useful for development or testing scenarios.
    /// </remarks>
    public sealed class NullJwtBearerQueryStringBehavior : JwtBearerQueryStringBehaviorBase {

        /// <summary>
        ///   Performs no mutations to the <see cref="HttpContext" /> of the user's web request.
        /// </summary>
        protected override void ApplyImpl(
            HttpContext context,
            JwtBearerQueryStringOptions options) {}

    }

}