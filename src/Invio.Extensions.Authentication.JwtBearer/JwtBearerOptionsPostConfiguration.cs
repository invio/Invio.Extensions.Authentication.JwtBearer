using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Invio.Extensions.Authentication.JwtBearer {

    /// <summary>
    ///   Updates the <see cref="JwtBearerOptions" /> to enable query string-based
    ///   authentication with JWT bearer tokens.
    /// </summary>
    public sealed class JwtBearerOptionsPostConfiguration : IPostConfigureOptions<JwtBearerOptions> {

        private IOptions<JwtBearerQueryStringOptions> options { get; }

        /// <summary>
        ///   Instantiates a post configure event handler that will mutate the
        ///   <see cref="JwtBearerOptions" /> after it has been configured
        ///   for use by the web service.
        /// </summary>
        /// <param name="options">
        ///   The configuration settings for how the request pipeline will be analyzed
        ///   and mutated to support query string-based authentication.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="options" /> is null.
        /// </exception>
        public JwtBearerOptionsPostConfiguration(IOptions<JwtBearerQueryStringOptions> options) {
            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            this.options = options;
        }

        /// <summary>
        ///   Mutates the <see cref="JwtBearerOptions" /> such that it will now
        ///   look for access tokens in the query string, performing authentication
        ///   with them as it would if the token were presented in the traditional
        ///   'Authorization' header.
        /// </summary>
        public void PostConfigure(string name, JwtBearerOptions bearerOptions) {
            if (bearerOptions == null) {
                throw new ArgumentNullException(nameof(bearerOptions));
            }

            bearerOptions.AddQueryStringAuthentication(
                this.options.Value.QueryStringParameterName
            );
        }

    }

}