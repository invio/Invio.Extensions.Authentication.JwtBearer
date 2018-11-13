using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Moq;
using Xunit;

namespace Invio.Extensions.Authentication.JwtBearer {

    public abstract class JwtBearerEventsWrapperBaseTests {

        [Fact]
        public async Task AuthenticationFailed_Wraps() {

            // Arrange

            var inner = Mock.Of<JwtBearerEvents>();
            var wrapper = this.CreateJwtBearerEvents(inner);
            var context = new DefaultAuthenticationFailedContext();

            // Act

            await wrapper.AuthenticationFailed(context);

            // Assert

            Mock.Get(inner).Verify(events => events.AuthenticationFailed(context));
        }

        [Fact]
        public async Task MessageReceived_Wraps() {

            // Arrange

            var inner = Mock.Of<JwtBearerEvents>();
            var wrapper = this.CreateJwtBearerEvents(inner);
            var context = new DefaultMessageReceivedContext();

            // Act

            await wrapper.MessageReceived(context);

            // Assert

            Mock.Get(inner).Verify(events => events.MessageReceived(context));
        }

        [Fact]
        public async Task TokenValidated() {

            // Arrange

            var inner = Mock.Of<JwtBearerEvents>();
            var wrapper = this.CreateJwtBearerEvents(inner);
            var context = new DefaultTokenValidatedContext();

            // Act

            await wrapper.TokenValidated(context);

            // Assert

            Mock.Get(inner).Verify(events => events.TokenValidated(context));
        }

        [Fact]
        public async Task Challenge_Wraps() {

            // Arrange

            var inner = Mock.Of<JwtBearerEvents>();
            var wrapper = this.CreateJwtBearerEvents(inner);
            var context = new DefaultJwtBearerChallengeContext();

            // Act

            await wrapper.Challenge(context);

            // Assert

            Mock.Get(inner).Verify(events => events.Challenge(context));
        }

        protected abstract JwtBearerEvents CreateJwtBearerEvents(JwtBearerEvents inner);

    }

}
