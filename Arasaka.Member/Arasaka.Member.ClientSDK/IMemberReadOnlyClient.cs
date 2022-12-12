using Arasaka.Member.ClientSDK.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Arasaka.Member.ClientSDK
{
    public interface IMemberReadOnlyClient
    {
        Task<MemberInformation> GetAsync(long memberId, CancellationToken cancellationToken);

        Task<IEnumerable<MemberInformation>> ListAsync(ListMembersFilter listMembersFilter, CancellationToken cancellationToken);



    }
}
