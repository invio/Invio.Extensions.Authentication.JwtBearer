using System;
using System.Collections.Generic;
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

        public static IEnumerable<object[]> Apply_EmptyQueryStringIsIgnored_MemberData {
            get {
                yield return new object[] { null };
                yield return new object[] { new QueryString("?") };
                yield return new object[] { QueryString.Empty };
                yield return new object[] { new DefaultHttpContext().Request.QueryString };
            }
        }

        [Theory]
        [MemberData(nameof(Apply_EmptyQueryStringIsIgnored_MemberData))]
        public void Apply_EmptyQueryStringIsIgnored(QueryString original) {

            // Arrange

            var behavior = this.CreateBehavior();
            var context = new DefaultHttpContext();
            var options = new JwtBearerQueryStringOptions();

            context.Request.QueryString = original;

            // Act

            behavior.Apply(context, options);

            // Assert

            Assert.Equal(original, context.Request.QueryString);
        }

        protected abstract IJwtBearerQueryStringBehavior CreateBehavior();

    }

}