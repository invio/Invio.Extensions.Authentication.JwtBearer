using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Invio.Extensions.Authentication.JwtBearer {

    /// <summary>
    ///   Mutates the <see cref="HttpContext" /> of a given request based upon the behavior
    ///   defined via the <see cref="JwtBearerQueryStringOptions" />.
    /// </summary>
    public sealed class JwtBearerQueryStringMiddleware {

        private RequestDelegate next { get; }
        private IOptions<JwtBearerQueryStringOptions> options { get; }

        /// <summary>
        ///   Instantiates a new instance of the <see cref="JwtBearerQueryStringMiddleware" />
        ///   with the next step in the request pipeline and the
        ///   <see cref="JwtBearerQueryStringOptions" /> that define how the web request
        ///   will be mutated in order to perform the desired query string management behavior.
        /// </summary>
        /// <param name="next">
        ///   The next step in the web request pipeline.
        /// </param>
        /// <param name="options">
        ///   The configuration settings for how the request pipeline will be analyzed
        ///   and mutated to support query string-based authentication.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="next" /> or <paramref name="options" /> is null.
        /// </exception>
        public JwtBearerQueryStringMiddleware(
            RequestDelegate next,
            IOptions<JwtBearerQueryStringOptions> options) {

            if (next == null) {
                throw new ArgumentNullException(nameof(next));
            } else if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            this.next = next;
            this.options = options;
        }

        /// <summary>
        ///   Runs this step in the pipeline, mutating the <see cref="HttpContext" />
        ///   in accordance to the configuration defined via the injected
        ///   <see cref="JwtBearerQueryStringOptions" />.
        /// </summary>
        /// <param name="context">
        ///   The underlying <see cref="HttpContext" /> for the web request being
        ///   processed by this web service.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="context" /> is null.
        /// </exception>
        public Task Invoke(HttpContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }

            this.options.Value.QueryStringBehavior?.Apply(context, options.Value);

            return this.next(context);
        }

    }


}