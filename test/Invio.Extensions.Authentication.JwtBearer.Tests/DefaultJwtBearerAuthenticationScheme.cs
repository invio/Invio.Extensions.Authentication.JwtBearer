using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Invio.Extensions.Authentication.JwtBearer {

    public class DefaultJwtBearerAuthenticationScheme : AuthenticationScheme {

        public DefaultJwtBearerAuthenticationScheme() :
            base(JwtBearerDefaults.AuthenticationScheme,
                 null,
                 typeof(IAuthenticationHandler)) {}

    }

}
