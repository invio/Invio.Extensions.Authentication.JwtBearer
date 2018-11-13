using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Invio.Extensions.Authentication.JwtBearer {

    public class DefaultJwtBearerChallengeContext : JwtBearerChallengeContext {

        public DefaultJwtBearerChallengeContext() :
            base(new DefaultHttpContext(),
                 new DefaultJwtBearerAuthenticationScheme(),
                 new JwtBearerOptions(),
                 new AuthenticationProperties()) {}


    }

}
