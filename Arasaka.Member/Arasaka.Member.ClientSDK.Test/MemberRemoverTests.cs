using Xunit;
using NSubstitute;
using Arasaka.Member.ClientSDK;
using Arasaka.Member.ClientSDK.Models;
using FluentAssertions;

namespace Arasaka.Member.ClientApp.Test
{
    public class MemberRemoverTests
    {
        private MemberClient _memberClient = Substitute.For<MemberClient>();

        private MemberRemover GetSystemUnderTest()
        {
            return new MemberRemover(_memberClient);
        }

        [Fact]
        public async void MemberRemover_Remove()
        {
            // arrange
            var memberRemover = GetSystemUnderTest();
            long memberId = 9527;
            var member = new MemberInformation() { Id = memberId };
            var cancelToken = System.Threading.CancellationToken.None;
            _memberClient.GetAsync(memberId, cancelToken).Returns(member);

            // act
            var action = async () => await _memberClient.RemoveAsync(memberId, cancelToken);

            // assert
            await action.Should().NotThrowAsync();
        }
    }
}