using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Invio.Extensions.Authentication.JwtBearer {

    /// <summary>
    ///   Boilerplate wrapper implementation for <see cref="JwtBearerEvents" />
    ///   event handler class to enable composition of event handling behavior.
    /// </summary>
    /// <remarks>
    ///   This is abstract with a protected constructor as it should only enable
    ///   true wrappers to implement their desired functionality without having
    ///   to provide distracting invocations to the wrapped <see cref="JwtBearerEvents" />.
    /// </remarks>
    public abstract class JwtBearerEventsWrapperBase : JwtBearerEvents {

        private JwtBearerEvents inner { get; }

        /// <summary>
        ///   Creates a <see cref="JwtBearerEvents" /> implementation which
        ///   does nothing but call the injected wrapped implementation by default.
        /// </summary>
        /// <param name="inner">
        ///   The base <see cref="JwtBearerEvents" /> that will be invoked after the
        ///   inheriting wrapper performs its side effects.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="inner" /> is null.
        /// </exception>
        protected JwtBearerEventsWrapperBase(JwtBearerEvents inner) {
            if (inner == null) {
                throw new ArgumentNullException(nameof(inner));
            }

            this.inner = inner;
        }

        /// <summary>
        ///   Invoked if exceptions are thrown during request processing.
        ///   The exceptions will be re-thrown after this event unless suppressed.
        /// </summary>
        public override Task AuthenticationFailed(AuthenticationFailedContext context) {
            return inner.AuthenticationFailed(context);
        }

        /// <summary>
        ///   Invoked when a protocol message is first received.
        /// </summary>
        public override Task MessageReceived(MessageReceivedContext context) {
            return inner.MessageReceived(context);
        }

        /// <summary>
        ///   Invoked after the security token has passed validation
        ///   and a ClaimsIdentity has been generated.
        /// </summary>
        public override Task TokenValidated(TokenValidatedContext context) {
            return inner.TokenValidated(context);
        }

        /// <summary>
        ///   Invoked before a challenge is sent back to the caller.
        /// </summary>
        public override Task Challenge(JwtBearerChallengeContext context) {
            return inner.Challenge(context);
        }

    }

}
