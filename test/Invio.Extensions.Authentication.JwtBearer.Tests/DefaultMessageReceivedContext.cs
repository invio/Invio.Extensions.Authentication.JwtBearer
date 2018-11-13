using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Invio.Extensions.Authentication.JwtBearer {

    public class DefaultMessageReceivedContext : MessageReceivedContext {

        public DefaultMessageReceivedContext() :
            base(new DefaultHttpContext(),
                 new DefaultJwtBearerAuthenticationScheme(),
                 new JwtBearerOptions()) {}

    }

}
