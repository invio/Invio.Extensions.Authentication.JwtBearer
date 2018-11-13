# Invio.Extensions.Authentication.JwtBearer

This library extends the functionality of Microsoft's base implementation of JWT bearer handling found in [`Microsoft.AspNetCore.Authentication.JwtBearer`](https://github.com/aspnet/Security/tree/master/src/Microsoft.AspNetCore.Authentication.JwtBearer). Currently, the only thing it adds is the ability to enable ["URI Query Parameter" authentication as described in RFC 6750](https://tools.ietf.org/html/rfc6750#section-2.3).

## Installation
The latest version of this package is available on NuGet. To install, run the following command:

```
PM> dotnet add package Invio.Extensions.Authentication.JwtBearer
```

### Enable `Bearer` Tokens in URI Query Parameters

The way most users are taught to use `Bearer` tokens is via the "Authorization Request Header Field," [as defined in RFC 6750](https://tools.ietf.org/html/rfc6750#section-2.1). This makes sense, as it is OAuth 2.0's recommended way to consume these tokens securely via an API. However, if an API consumer wishes to render HTML with image tags (e.g. `<img src="https://api.example.com/image"/>`) or download links (e.g. `<a href="https://api.example.com/file">download</a>`) directly from the API, they'll be blocked as there is no way to include a request header in these situations.

Fortunately, [RFC 6750](https://tools.ietf.org/html/rfc6750) specifies an alternative way to retrieve `Bearer` tokens [as a URI Query parameter](https://tools.ietf.org/html/rfc6750#section-2.3). It states that a user can specify the ``"access_token"`` query string parameter with a value of the `Bearer` token, like so:

> GET /resource?access_token=eyJhbGciOiJIUzI1NiJ9.e30.bEionjP7X5J_VkgbZPrgfmAbNskfY4eG97AIRGA5kGg<br/>
> Host: api.example.com

This library simplifies the ability to include this method of authentication within a ASP.NET Core WebAPI. When you follow [the steps to add JWT Bearer authorization](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/limitingidentitybyscheme) to your web application, your `Startup.ConfigureServices` method will look something like this:

```cs
using Microsoft.AspNetCore.Authentication.JwtBearer;

public void ConfigureServices(IServiceCollection services) {
    services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(
            options => {
                var authentication = this.configuration.GetSection("Authentication");

                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidIssuers = authentication["Issuer"],
                    ValidAudience = authentication["ClientId"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(authentication["ClientSecret"])
                    )
                };
            }
        );

    // ... other code omitted for brevity
}
```

Once you install the package, you can add it to your [`JwtBearerOptions`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.jwtbearer.jwtbeareroptions) configuration by calling one of
the [`JwtBearerOptionsExtensions.AddQueryStringAuthentication`](https://github.com/invio/Invio.Extensions.Authentication.JwtBearer/blob/master/src/Invio.Extensions.Authentication.JwtBearer/JwtBearerOptionsExtensions.cs) extension methods.

```cs
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Invio.Extensions.Authentication.JwtBearer;

public void ConfigureServices(IServiceCollection services) {
    services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(
            options => {
                var authentication = this.configuration.GetSection("Authentication");

                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidIssuers = authentication["Issuer"],
                    ValidAudience = authentication["ClientId"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(authentication["ClientSecret"])
                    )
                };

                // Adds "URI Query Parameters" authentication

                options.AddQueryStringAuthentication();

                // Alternatively:
                //   options.AddQueryStringAuthentication("custom-parameter-name")
            }
        );

    // ... other code omitted for brevity
}
```

Now you can call your endpoints using URI query parameter authentication. You can test it out like so with [curl](https://en.wikipedia.org/wiki/CURL), or some other tool of your choosing.

```
curl -I https://api.example.com/resource?access_token=eyJhbGciOiJIUzI1NiJ9.e30.bEionjP7X5J_VkgbZPrgfmAbNskfY4eG97AIRGA5kGg
```

That's it. <3