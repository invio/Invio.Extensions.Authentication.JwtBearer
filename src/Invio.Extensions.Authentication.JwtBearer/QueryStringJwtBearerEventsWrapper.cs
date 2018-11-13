using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;

namespace Invio.Extensions.Authentication.JwtBearer {

    public sealed class QueryStringJwtBearerEventsWrapper : JwtBearerEventsWrapperBase {

        public static String DefaultQueryStringKey { get; } = "bearer";
        public string QueryStringKey { get; }

        public QueryStringJwtBearerEventsWrapper(JwtBearerEvents inner) :
            this(inner, DefaultQueryStringKey) {}

        public QueryStringJwtBearerEventsWrapper(JwtBearerEvents inner, string queryStringKey) :
            base(inner) {

            if (queryStringKey == null) {
                throw new ArgumentNullException(nameof(queryStringKey));
            } else if (String.IsNullOrWhiteSpace(queryStringKey)) {
                throw new ArgumentException(
                    $"The '{nameof(queryStringKey)}' cannot be null or whitespace.",
                    nameof(queryStringKey)
                );
            }

            this.QueryStringKey = queryStringKey;
        }

        public override Task MessageReceived(MessageReceivedContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }

            StringValues values;

            if (!context.Request.Query.TryGetValue(this.QueryStringKey, out values)) {
                return base.MessageReceived(context);
            }

            if (values.Count > 1) {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Fail(
                    $"Only one '{this.QueryStringKey}' query string parameter can be " +
                    $"defined. However, {values.Count:N0} were included in the request."
                );

                return base.MessageReceived(context);
            }

            var token = values.Single();

            if (String.IsNullOrWhiteSpace(token)) {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Fail(
                    $"The '{this.QueryStringKey}' query string parameter was defined, " +
                    $"but a value to represent the token was not included."
                );

                return base.MessageReceived(context);
            }

            context.Token = token;

            return base.MessageReceived(context);
        }

    }

}