using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Invio.Extensions.Authentication.JwtBearer {

    public abstract class JwtBearerEventsWrapperBase : JwtBearerEvents {

        private JwtBearerEvents inner { get; }

        public JwtBearerEventsWrapperBase(JwtBearerEvents inner) {
            if (inner == null) {
                throw new ArgumentNullException(nameof(inner));
            }

            this.inner = inner;
        }

        public override Task AuthenticationFailed(AuthenticationFailedContext context) {
            return inner.AuthenticationFailed(context);
        }

        public override Task MessageReceived(MessageReceivedContext context) {
            return inner.MessageReceived(context);
        }

        public override Task TokenValidated(TokenValidatedContext context) {
            return inner.TokenValidated(context);
        }

        public override Task Challenge(JwtBearerChallengeContext context) {
            return inner.Challenge(context);
        }

    }

}
