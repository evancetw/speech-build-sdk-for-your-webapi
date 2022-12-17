using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Arasaka.Member.ClientSDK.Models;

namespace Arasaka.Member.ClientSDK
{
    /// <summary>
    /// Member API 客戶端
    /// </summary>
    public interface IMemberClient2 : IMemberWriteClient, IMemberReadOnlyClient
    {


    }

    public interface IMemberClient
    {
        Task<MemberInformation> GetAsync(long memberId, CancellationToken cancellationToken);

        Task<IEnumerable<MemberInformation>> ListAsync(ListMembersFilter listMembersFilter, CancellationToken cancellationToken);

        Task<MemberInformation> SignUpAsync(MemberSignUpForm memberSignUpForm, CancellationToken cancellationToken);

        Task VerifyAsync(long memberId, CancellationToken cancellationToken);

        Task RestrictAsync(long memberId, CancellationToken cancellationToken);

        Task AllowAsync(long memberId, CancellationToken cancellationToken);

        Task BanAsync(long memberId, CancellationToken cancellationToken);

        Task PermitAsync(long memberId, CancellationToken cancellationToken);

        Task RemoveAsync(long memberId, CancellationToken cancellationToken);
    }
}
