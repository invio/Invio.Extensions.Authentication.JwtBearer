using System;
using Invio.Xunit;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Invio.Extensions.Authentication.JwtBearer {

    [UnitTest]
    public sealed class RedactJwtBearerQueryStringBehaviorTests : JwtBearerQueryStringBehaviorBaseTests {

        [Fact]
        public void Apply_QueryStringNotPresent() {

            // Arrange

            var behavior = this.CreateBehavior();

            var context = new DefaultHttpContext();
            var options = new JwtBearerQueryStringOptions();

            var queryString = new QueryString("?ParameterName=Value1&ParameterName=Value2");
            context.Request.QueryString = queryString;

            // Act

            behavior.Apply(context, options);

            // Assert

            Assert.Equal(queryString, context.Request.QueryString);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("access_token")]
        [InlineData("custom-parameter")]
        public void Apply_QueryStringAppliedOnce(string parameterName) {

            // Arrange

            var behavior = this.CreateBehavior();

            var context = new DefaultHttpContext();
            var options = new JwtBearerQueryStringOptions {
                QueryStringParameterName =
                    parameterName ??
                    JwtBearerQueryStringOptions.DefaultQueryStringParameterName
            };

            const string token = "MyToken";
            var queryString = new QueryString($"?{options.QueryStringParameterName}={token}");
            context.Request.QueryString = queryString;

            // Act

            behavior.Apply(context, options);

            // Assert

            Assert.Contains(
                $"{options.QueryStringParameterName}={RedactJwtBearerQueryStringBehavior.DefaultRedactedValue}",
                context.Request.QueryString.Value
            );

            Assert.DoesNotContain(token, context.Request.QueryString.Value);
        }

        [Fact]
        public void Apply_QueryStringAppliedMultipleTimes() {

            // Arrange

            var behavior = this.CreateBehavior();

            var context = new DefaultHttpContext();
            var options = new JwtBearerQueryStringOptions();

            const string token = "MyToken";
            var queryString = new QueryString($"?access_token={token}1&access_token={token}2");
            context.Request.QueryString = queryString;

            // Act

            behavior.Apply(context, options);

            // Assert

            Assert.Contains(
                "access_token=" + RedactJwtBearerQueryStringBehavior.DefaultRedactedValue,
                context.Request.QueryString.Value
            );

            Assert.DoesNotContain(token, context.Request.QueryString.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Apply_NullRedactionValuesSupported(string redactedValue) {

            // Arrange

            var behavior = this.CreateBehavior(redactedValue);

            var context = new DefaultHttpContext();
            var options = new JwtBearerQueryStringOptions();

            const string token = "MyOtherToken";
            var queryString = new QueryString($"?access_token={token}");
            context.Request.QueryString = queryString;

            // Act

            behavior.Apply(context, options);

            // Assert

            Assert.Contains("access_token=" + redactedValue, context.Request.QueryString.Value);
            Assert.DoesNotContain(token, context.Request.QueryString.Value);
        }

        protected override IJwtBearerQueryStringBehavior CreateBehavior() {
            return this.CreateBehavior(RedactJwtBearerQueryStringBehavior.DefaultRedactedValue);
        }

        private IJwtBearerQueryStringBehavior CreateBehavior(string redactedValue) {
            return new RedactJwtBearerQueryStringBehavior(redactedValue);
        }

    }

}