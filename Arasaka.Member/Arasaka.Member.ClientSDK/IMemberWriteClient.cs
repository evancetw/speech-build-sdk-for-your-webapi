using Arasaka.Member.ClientSDK.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Arasaka.Member.ClientSDK
{
    /// <summary>
    /// Member API 客戶端
    /// </summary>
    public interface IMemberWriteClient
    {
        Task<MemberInformation> SignUpAsync(MemberSignUpForm memberSignUpForm, CancellationToken cancellationToken);

        Task VerifyAsync(long memberId, CancellationToken cancellationToken);

        Task RestrictAsync(long memberId, CancellationToken cancellationToken);

        Task AllowAsync(long memberId, CancellationToken cancellationToken);

        Task BanAsync(long memberId, CancellationToken cancellationToken);

        Task PermitAsync(long memberId, CancellationToken cancellationToken);

        Task RemoveAsync(long memberId, CancellationToken cancellationToken);
    }
}
