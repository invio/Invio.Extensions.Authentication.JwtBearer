using System;
using System.Net;
using System.Threading.Tasks;
using Invio.Xunit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Invio.Extensions.Authentication.JwtBearer {

    [UnitTest]
    public sealed class QueryStringJwtBearerEventsWrapperTests : JwtBearerEventsWrapperBaseTests {

        [Fact]
        public void Constructor_DefaultQueryStringParameterName_NullInner() {

            // Arrange

            JwtBearerEvents inner = null;

            // Act

            var exception = Record.Exception(
                () => new QueryStringJwtBearerEventsWrapper(inner)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Constructor_CustomQueryStringParameterName_NullInner() {

            // Arrange

            JwtBearerEvents inner = null;

            // Act

            var exception = Record.Exception(
                () => new QueryStringJwtBearerEventsWrapper(inner, "myCustomQueryStringParameterName")
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Constructor_CustomQueryStringParameterName_NullParameterName() {

            // Arrange

            var inner = new JwtBearerEvents();

            // Act

            var exception = Record.Exception(
                () => new QueryStringJwtBearerEventsWrapper(inner, null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_WithCustomQueryStringParameterName_InvalidParameterName(
            string queryStringParameterName) {

            // Arrange

            var inner = new JwtBearerEvents();

            // Act

            var exception = Record.Exception(
                () => new QueryStringJwtBearerEventsWrapper(inner, queryStringParameterName)
            );

            // Assert

            Assert.IsType<ArgumentException>(exception);

            Assert.Equal(
                $"The 'queryStringParameterName' cannot be null or whitespace." +
                Environment.NewLine + "Parameter name: queryStringParameterName",
                exception.Message
            );
        }

        [Fact]
        public async Task MessageReceived_NullContext() {

            // Arrange

            var inner = new JwtBearerEvents();
            var events = this.CreateJwtBearerEvents(inner);

            // Act

            var exception = await Record.ExceptionAsync(
                () => events.MessageReceived(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("?")]
        [InlineData("?another_token=foo")]
        public async Task MessageReceived_NoQueryStringParameterName(String queryString) {

            // Arrange

            var inner = Mock.Of<JwtBearerEvents>();
            var events = this.CreateJwtBearerEvents(inner);
            var context = new DefaultMessageReceivedContext();
            context.Request.QueryString = new QueryString(queryString);

            // Act

            await events.MessageReceived(context);

            // Assert

            Assert.Null(context.Token);
            Assert.Null(context.Result);
            Mock.Get(inner).Verify(e => e.MessageReceived(context));
        }

        [Theory]
        [InlineData("")]
        [InlineData("=")]
        [InlineData("=%20")]
        public async Task MessageReceived_WhiteSpaceToken(String value) {

            // Arrange

            var inner = Mock.Of<JwtBearerEvents>();
            var events = this.CreateQueryStringJwtBearerEvents(inner);
            var context = new DefaultMessageReceivedContext();
            var queryString = $"?{events.QueryStringParameterName}{value}";

            context.Request.QueryString = new QueryString(queryString);

            // Act

            await events.MessageReceived(context);

            // Assert

            Assert.Null(context.Token);

            Assert.NotNull(context.Result);
            Assert.Equal(
                $"The '{events.QueryStringParameterName}' query string parameter was " +
                $"defined, but a value to represent the token was not included.",
                context.Result.Failure?.Message
            );

            Assert.Equal((int)HttpStatusCode.Unauthorized, context.Response.StatusCode);
            Mock.Get(inner).Verify(e => e.MessageReceived(context));
        }

        [Fact]
        public async Task MessageReceived_MoreThanOneToken() {

            // Arrange

            var inner = Mock.Of<JwtBearerEvents>();
            var events = this.CreateQueryStringJwtBearerEvents(inner);
            var context = new DefaultMessageReceivedContext();
            var queryString = $"?{events.QueryStringParameterName}=foo&{events.QueryStringParameterName}=bar";

            context.Request.QueryString = new QueryString(queryString);

            // Act

            await events.MessageReceived(context);

            // Assert

            Assert.Null(context.Token);

            Assert.NotNull(context.Result?.Failure);
            Assert.Equal(
                $"Only one '{events.QueryStringParameterName}' query string parameter " +
                $"can be defined. However, 2 were included in the request.",
                context.Result.Failure.Message
            );

            Assert.Equal((int)HttpStatusCode.Unauthorized, context.Response.StatusCode);
            Mock.Get(inner).Verify(e => e.MessageReceived(context));
        }

        [Fact]
        public async Task MessageReceived_ValidDefaultToken() {

            // Arrange

            var inner = Mock.Of<JwtBearerEvents>();
            var events = this.CreateQueryStringJwtBearerEvents(inner);
            var context = new DefaultMessageReceivedContext();

            const string token = "foo";
            var queryString =
                $"?{QueryStringJwtBearerEventsWrapper.DefaultQueryStringParameterName}={token}";

            context.Request.QueryString = new QueryString(queryString);

            // Act

            await events.MessageReceived(context);

            // Assert

            Assert.Equal(token, context.Token);
            Assert.Null(context.Result);
            Mock.Get(inner).Verify(e => e.MessageReceived(context));
        }

        [Theory]
        [InlineData("bearer")]
        [InlineData("id-token")]
        [InlineData("with space")]
        public async Task MessageReceived_ValidCustomToken(string customQueryStringParameterName) {

            // Arrange

            var inner = Mock.Of<JwtBearerEvents>();
            var events = new QueryStringJwtBearerEventsWrapper(inner, customQueryStringParameterName);
            var context = new DefaultMessageReceivedContext();

            const string token = "foo";
            var queryString = $"?{customQueryStringParameterName}={token}";

            context.Request.QueryString = new QueryString(queryString);

            // Act

            await events.MessageReceived(context);

            // Assert

            Assert.Equal(token, context.Token);
            Assert.Null(context.Result);
            Mock.Get(inner).Verify(e => e.MessageReceived(context));
        }

        protected override JwtBearerEvents CreateJwtBearerEvents(JwtBearerEvents inner) {
            return this.CreateQueryStringJwtBearerEvents(inner);
        }

        private QueryStringJwtBearerEventsWrapper CreateQueryStringJwtBearerEvents(
            JwtBearerEvents inner) {

            return new QueryStringJwtBearerEventsWrapper(inner);
        }

    }

}
