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
        .AddJwtBearer(options => options.TokenValidationParameters = /* ... */)
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
    app.UseAuthentication();
    app.UseMvc();
}
```

Once you install the package, you can apply it to your [`JwtBearerOptions`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.jwtbearer.jwtbeareroptions) configuration by calling one of the [`AuthenticationBuilderExtensions.AddJwtBearerQueryStringAuthentication`](https://github.com/invio/Invio.Extensions.Authentication.JwtBearer/blob/master/src/Invio.Extensions.Authentication.JwtBearer/AuthenticationBuilderExtensions.cs) extension methods on the JWT Bearer's `AuthenticationBuilder` as well as applying the [`JwtBearerQueryStringMiddleware`](https://github.com/invio/Invio.Extensions.Authentication.JwtBearer/blob/master/src/Invio.Extensions.Authentication.JwtBearer/JwtBearerQueryStringMiddleware.cs) by calling the [`ApplicationBuilderExtensions.UseJwtBearerQueryString`](https://github.com/invio/Invio.Extensions.Authentication.JwtBearer/blob/master/src/Invio.Extensions.Authentication.JwtBearer/ApplicationBuilderExtensions.cs) extension method.

```cs
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Invio.Extensions.Authentication.JwtBearer;

public void ConfigureServices(IServiceCollection services) {
    services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => options.TokenValidationParameters = /* ... */)

        // This example shows the default options. You can set them to
        // whatever you like or you can even leave out the lambda altogether.
        .AddJwtBearerQueryStringAuthentication(
            (JwtBearerQueryStringOptions options) => {
                options.QueryStringParameterName = "access_token";
                options.QueryStringBehavior = QueryStringBehaviors.Redact;
            }
        );        
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
    app.UseAuthentication();
    // This should come after authentication but before any logging middleware.
    app.UseJwtBearerQueryString();
    app.UseMvc();
}
```

### Configuring the ``

By default, this will enable users to send their JWT bearer tokens using the `"access_token"` query string parameter, and the value of that token will be redacted via the middleware. The string `"(REDACTED)"` is put in the token's place before the [`HttpContext`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext) representing the user's web request is moved up the pipeline. For more information on how to configure this behavior, see the [`JwtBearerQueryStringOptions`](https://github.com/invio/Invio.Extensions.Authentication.JwtBearer/blob/master/src/Invio.Extensions.Authentication.JwtBearer/JwtBearerQueryStringOptions.cs).

### Wrapping Up

Now you can call your endpoints using URI query parameter authentication. You can test it out like so with [cURL](https://en.wikipedia.org/wiki/CURL), or some other tool of your choosing.

```
curl -I https://api.example.com/resource?access_token=eyJhbGciOiJIUzI1NiJ9.e30.bEionjP7X5J_VkgbZPrgfmAbNskfY4eG97AIRGA5kGg
```

That's it. <3