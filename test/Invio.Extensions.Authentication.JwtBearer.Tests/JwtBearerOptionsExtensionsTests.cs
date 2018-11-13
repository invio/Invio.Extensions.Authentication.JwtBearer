using System;
using Invio.Xunit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Xunit;

namespace Invio.Extensions.Authentication.JwtBearer {

    [UnitTest]
    public sealed class JwtBearerOptionsExtensionsTests {

        [Fact]
        public static void AddQueryStringAuthentication_DefaultQueryStringParameter_NullOptions() {

            // Arrange

            JwtBearerOptions options = null;

            // Act

            var exception = Record.Exception(
                () => options.AddQueryStringAuthentication()
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public static void AddQueryStringAuthentication_DefaultQueryStringParameter() {

            // Arrange

            var options = new JwtBearerOptions();

            // Act

            options.AddQueryStringAuthentication();

            // Assert

            Assert.NotNull(options.Events);
            var events = Assert.IsType<QueryStringJwtBearerEventsWrapper>(options.Events);

            Assert.Equal(
                QueryStringJwtBearerEventsWrapper.DefaultQueryStringParameterName,
                events.QueryStringParameterName
            );
        }

        [Fact]
        public static void AddQueryStringAuthentication_CustomQueryStringParameter_NullOptions() {

            // Arrange

            JwtBearerOptions options = null;

            // Act

            var exception = Record.Exception(
                () => options.AddQueryStringAuthentication("query-string-parameter-name")
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public static void AddQueryStringAuthentication_CustomQueryStringParameter() {

            // Arrange

            var options = new JwtBearerOptions();
            const string queryStringParameterName = "query-string-parameter-name";

            // Act

            options.AddQueryStringAuthentication(queryStringParameterName);

            // Assert

            Assert.NotNull(options.Events);
            var events = Assert.IsType<QueryStringJwtBearerEventsWrapper>(options.Events);
            Assert.Equal(queryStringParameterName, events.QueryStringParameterName);
        }

    }

}