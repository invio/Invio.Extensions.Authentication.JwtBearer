using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;

namespace Invio.Extensions.Authentication.JwtBearer {

    /// <summary>
    ///   Creates a wrapper around an inner <see cref="JwtBearerEvents" /> that fetches
    ///   the API consumer's bearer token out of the query string if it is present.
    ///   If the query string parameter is not present, this wrapper does nothing.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     This can be used when the application consuming a .NET Core WebAPI needs to
    ///     enable browser sessioins to download images and/or other binary files directly.
    ///   </para>
    ///   <para>
    ///     If a token is specified in both as a query string parameter and in the
    ///     'Authorization' header, the query string parameter "wins" and will be used
    ///     even if it is invalid and the header's token is valid.
    ///   </para>
    /// </remarks>
    public class QueryStringJwtBearerEventsWrapper : JwtBearerEventsWrapperBase {

        /// <summary>
        ///   This is the default name of the query string parameter that will be
        ///   sought before the 'Authorization' header is parsed using the 'Bearer'
        ///   authentication scheme.
        /// </summary>
        /// <remarks>
        ///   This is the default specified in RFC 6750.
        ///   <see href="https://tools.ietf.org/html/rfc6750#section-2.3" />
        /// </remarks>
        public static String DefaultQueryStringParameterName { get; } = "access_token";

        /// <summary>
        ///   This is the actual query string parameter that will be sought in all
        ///   requests to perform authentication. If found, its value is used for
        ///   processing bearer authentication for this web request.
        /// </summary>
        public string QueryStringParameterName { get; }

        /// <summary>
        ///   Wraps an instance of <see cref="JwtBearerEvents" /> with a behavior
        ///   that checks for a token in the query string with a name of "bearer".
        /// </summary>
        /// <param name="inner">
        ///   A base <see cref="JwtBearerEvents" /> implementation that will gain
        ///   this additional query string inspection behavior.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="inner" /> is null.
        /// </exception>
        public QueryStringJwtBearerEventsWrapper(JwtBearerEvents inner) :
            this(inner, DefaultQueryStringParameterName) {}

        /// <summary>
        ///   Wraps an instance of <see cref="JwtBearerEvents" /> with a behavior
        ///   checks for a token in the query string with a name specified in the
        ///   <paramref name="queryStringParameterName" /> parameter.
        /// </summary>
        /// <param name="inner">
        ///   A base <see cref="JwtBearerEvents" /> implementation that will gain
        ///   this additional query string inspection behavior.
        /// </param>
        /// <param name="queryStringParameterName">
        ///   The name of the query string parameter that will be sought from requests
        ///   in order to extract a token.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="inner" /> or
        ///   <paramref name="queryStringParameterName" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   Thrown when <paramref name="queryStringParameterName" /> is an invalid name
        ///   for a query string parameter.
        /// </exception>
        public QueryStringJwtBearerEventsWrapper(JwtBearerEvents inner, string queryStringParameterName) :
            base(inner) {

            if (queryStringParameterName == null) {
                throw new ArgumentNullException(nameof(queryStringParameterName));
            } else if (String.IsNullOrWhiteSpace(queryStringParameterName)) {
                throw new ArgumentException(
                    $"The '{nameof(queryStringParameterName)}' cannot be null or whitespace.",
                    nameof(queryStringParameterName)
                );
            }

            this.QueryStringParameterName = queryStringParameterName;
        }

        /// <summary>
        ///   Checks the web request for the <see cref="QueryStringParameterName" /> in
        ///   the request's query string. If it is found, it fetches the token
        ///   and sets it on the provided <see cref="MessageReceivedContext" />.
        /// </summary>
        /// <param name="context">
        ///   The <see cref="MessageReceivedContext" /> that provides details
        ///   on the web request that was made. The query string, among other
        ///   properties relevant to the request, is stored on this value object.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="context" /> is null.
        /// </exception>
        public override Task MessageReceived(MessageReceivedContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }

            StringValues values;

            if (!context.Request.Query.TryGetValue(this.QueryStringParameterName, out values)) {
                return base.MessageReceived(context);
            }

            if (values.Count > 1) {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Fail(
                    $"Only one '{this.QueryStringParameterName}' query string parameter can " +
                    $"be defined. However, {values.Count:N0} were included in the request."
                );

                return base.MessageReceived(context);
            }

            var token = values.Single();

            if (String.IsNullOrWhiteSpace(token)) {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Fail(
                    $"The '{this.QueryStringParameterName}' query string parameter was " +
                    $"defined, but a value to represent the token was not included."
                );

                return base.MessageReceived(context);
            }

            context.Token = token;

            return base.MessageReceived(context);
        }

    }

}