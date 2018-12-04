using System;
using Invio.Xunit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Invio.Extensions.Authentication.JwtBearer {

    [UnitTest]
    public sealed class ApplicationBuilderExtensionsTests {

        [Fact]
        public static void UseJwtBearerQueryString_NullBuilder() {

            // Arrange

            IApplicationBuilder builder = null;

            // Act

            var exception = Record.Exception(
                () => builder.UseJwtBearerQueryString()
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public static void UseJwtBearerQueryString_ValidBuilder() {

            // Arrange

            var services = new ServiceCollection();

            services.Configure<JwtBearerQueryStringOptions>(
                _ => new JwtBearerQueryStringOptions()
            );

            var builder = new ApplicationBuilder(services.BuildServiceProvider());

            // Act

            var exception = Record.Exception(
                () => builder.UseJwtBearerQueryString()
            );

            // Assert

            Assert.Null(exception);
            Assert.NotNull(builder.Build());
        }

    }

}