using System;
using Invio.Xunit;
using Xunit;

namespace Invio.Extensions.Authentication.JwtBearer {

    [UnitTest]
    public sealed class QueryStringBehaviorsTests {

        [Fact]
        public void Redact_UsesDefaultRedactedValue() {

            // Act

            var behavior = QueryStringBehaviors.Redact;

            // Assert

            var typed = Assert.IsType<RedactJwtBearerQueryStringBehavior>(behavior);

            Assert.Equal(
                RedactJwtBearerQueryStringBehavior.DefaultRedactedValue,
                typed.RedactedValue
            );
        }

        [Fact]
        public void None_UsesNullBehavior() {

            // Act

            var behavior = QueryStringBehaviors.None;

            // Act

            Assert.IsType<NullJwtBearerQueryStringBehavior>(behavior);
        }

        [Fact]
        public void RedactWithValue_UsesProvidedRedactedValue() {

            // Arrange

            const string redactedValue = "Foo";

            // Act

            var behavior = QueryStringBehaviors.RedactWithValue(redactedValue);

            // Assert

            var typed = Assert.IsType<RedactJwtBearerQueryStringBehavior>(behavior);
            Assert.Equal(redactedValue, typed.RedactedValue);
        }

    }

}