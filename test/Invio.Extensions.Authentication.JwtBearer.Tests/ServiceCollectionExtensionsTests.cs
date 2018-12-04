using System;
using System.Linq;
using Invio.Xunit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Invio.Extensions.Authentication.JwtBearer {

    [UnitTest]
    public sealed class ServiceCollectionExtensionsTests {

        [Fact]
        public static void AddJwtBearerQueryStringAuthentication_NullServices_NoConfigureOptions() {

            // Arrange

            ServiceCollection services = null;

            // Act

            var exception = Record.Exception(
                () => services.AddJwtBearerQueryStringAuthentication()
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public static void AddJwtBearerQueryStringAuthentication_NullServices_WithConfigureOptions() {

            // Arrange

            ServiceCollection services = null;

            // Act

            var exception = Record.Exception(
                () => services.AddJwtBearerQueryStringAuthentication(_ => {})
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public static void AddJwtBearerQueryStringAuthentication_NullConfigureOptions() {

            // Arrange

            var services = new ServiceCollection();

            // Act

            var exception = Record.Exception(
                () => services.AddJwtBearerQueryStringAuthentication(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public static void AddJwtBearerQueryStringAuthentication_AddsPostConfigureJwtBearerOptions() {

            // Arrange

            var services = new ServiceCollection();

            // Act

            services.AddJwtBearerQueryStringAuthentication();

            // Assert

            Assert.Contains(
                typeof(IPostConfigureOptions<JwtBearerOptions>),
                services.Select(descriptor => descriptor.ServiceType)
            );
        }

        [Fact]
        public static void AddJwtBearerQueryStringAuthentication_AddsJwtBearerQueryStringOptions() {

            // Arrange

            var services = new ServiceCollection();

            // Act

            services.AddJwtBearerQueryStringAuthentication(
                options => {
                    options.QueryStringParameterName = "access_token";
                    options.QueryStringBehavior = QueryStringBehaviors.None;
                }
            );

            // Assert

            Assert.Contains(
                typeof(IConfigureOptions<JwtBearerQueryStringOptions>),
                services.Select(descriptor => descriptor.ServiceType)
            );
        }

    }

}