using System;
using System.Linq;
using Invio.Xunit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Invio.Extensions.Authentication.JwtBearer {

    [UnitTest]
    public sealed class AuthenticationBuilderExtensionsTests {

        [Fact]
        public static void AddJwtBearerQueryStringAuthentication_NullServices_NoConfigureOptions() {

            // Arrange

            AuthenticationBuilder builder = null;

            // Act

            var exception = Record.Exception(
                () => builder.AddJwtBearerQueryStringAuthentication()
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public static void AddJwtBearerQueryStringAuthentication_NullServices_WithConfigureOptions() {

            // Arrange

            AuthenticationBuilder builder = null;

            // Act

            var exception = Record.Exception(
                () => builder.AddJwtBearerQueryStringAuthentication(_ => {})
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public static void AddJwtBearerQueryStringAuthentication_NullConfigureOptions() {

            // Arrange

            var builder = new AuthenticationBuilder(new ServiceCollection());

            // Act

            var exception = Record.Exception(
                () => builder.AddJwtBearerQueryStringAuthentication(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public static void AddJwtBearerQueryStringAuthentication_AddsPostConfigureJwtBearerOptions() {

            // Arrange

            var builder = new AuthenticationBuilder(new ServiceCollection());

            // Act

            builder.AddJwtBearerQueryStringAuthentication();

            // Assert

            Assert.Contains(
                typeof(IPostConfigureOptions<JwtBearerOptions>),
                builder.Services.Select(descriptor => descriptor.ServiceType)
            );
        }

        [Fact]
        public static void AddJwtBearerQueryStringAuthentication_AddsJwtBearerQueryStringOptions() {

            // Arrange

            var builder = new AuthenticationBuilder(new ServiceCollection());

            // Act

            builder.AddJwtBearerQueryStringAuthentication(
                options => {
                    options.QueryStringParameterName = "access_token";
                    options.QueryStringBehavior = QueryStringBehaviors.None;
                }
            );

            // Assert

            Assert.Contains(
                typeof(IConfigureOptions<JwtBearerQueryStringOptions>),
                builder.Services.Select(descriptor => descriptor.ServiceType)
            );
        }

    }

}