using System;
using Invio.Xunit;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Invio.Extensions.Authentication.JwtBearer {

    public abstract class JwtBearerQueryStringBehaviorBaseTests {

        [Fact]
        public void Apply_NullContext() {

            // Arrange

            var behavior = this.CreateBehavior();
            var options = new JwtBearerQueryStringOptions();

            // Act

            var exception = Record.Exception(
                () => behavior.Apply(null, options)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Apply_NullOptions() {

            // Arrange

            var behavior = this.CreateBehavior();
            var context = new DefaultHttpContext();

            // Act

            var exception = Record.Exception(
                () => behavior.Apply(context, null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        protected abstract IJwtBearerQueryStringBehavior CreateBehavior();

    }

}