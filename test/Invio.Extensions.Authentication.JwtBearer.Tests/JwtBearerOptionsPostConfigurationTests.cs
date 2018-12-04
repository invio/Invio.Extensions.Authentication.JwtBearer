using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Invio.Xunit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Xunit;

namespace Invio.Extensions.Authentication.JwtBearer {

    [UnitTest]
    public sealed class JwtBearerOptionsPostConfigurationTests {

        [Fact]
        public void Constructor_NullOptions() {

            // Act

            var exception = Record.Exception(
                () => new JwtBearerOptionsPostConfiguration(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("name")]
        public void PostConfigure_NullBearerOptions(string name) {

            // Arrange

            var configuration = CreatePostConfiguration();

            // Act

            var exception = Record.Exception(
                () => configuration.PostConfigure(name, null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        public static IEnumerable<object[]> PostConfigure_AppliesEventsWrapper_MemberData {
            get {
                var tuples = ImmutableList.Create<(string, JwtBearerOptions)>(
                    ("name", new JwtBearerOptions { Events = null }),
                    (null, new JwtBearerOptions { Events = new JwtBearerEvents() })
                );

                return tuples.Select(tuple => new object[] { tuple.Item1, tuple.Item2 });
            }
        }

        [Theory]
        [MemberData(nameof(PostConfigure_AppliesEventsWrapper_MemberData))]
        public void PostConfigure_AppliesEventsWrapper(
            string name,
            JwtBearerOptions bearerOptions) {

            // Arrange

            var options = new JwtBearerQueryStringOptions();
            var configuration = CreatePostConfiguration(options);

            // Act

            configuration.PostConfigure(name, bearerOptions);

            // Assert

            var typed = Assert.IsType<QueryStringJwtBearerEventsWrapper>(bearerOptions.Events);
            Assert.Equal(options.QueryStringParameterName, typed.QueryStringParameterName);
        }

        private IPostConfigureOptions<JwtBearerOptions> CreatePostConfiguration(
            JwtBearerQueryStringOptions options = null) {

            return new JwtBearerOptionsPostConfiguration(
                Options.Create(options ?? new JwtBearerQueryStringOptions())
            );
        }

    }

}