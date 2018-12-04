using System;
using Invio.Xunit;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Invio.Extensions.Authentication.JwtBearer {

    [UnitTest]
    public sealed class NullJwtBearerQueryStringBehaviorTests :
        JwtBearerQueryStringBehaviorBaseTests {

        [Fact]
        public void Apply_QueryStringUnaffected() {

            // Arrange

            var behavior = this.CreateBehavior();

            var context = new DefaultHttpContext();
            var options = new JwtBearerQueryStringOptions();

            context.Request.QueryString.Add(options.QueryStringParameterName, "Token");
            var queryString = context.Request.QueryString.ToString();

            // Act

            behavior.Apply(context, options);

            // Assert

            Assert.Equal(queryString, context.Request.QueryString.ToString());
        }

        protected override IJwtBearerQueryStringBehavior CreateBehavior() {
            return new NullJwtBearerQueryStringBehavior();
        }

    }

}