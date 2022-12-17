using Arasaka.Member.ClientSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arasaka.Member.ClientApp
{
    public class MemberRemover
    {
        private MemberClient _memberClient;

        public MemberRemover(MemberClient memberClient)
        {
            _memberClient = memberClient;
        }

        public async Task RemoveAsync(long memberId, CancellationToken cancellationToken)
        {
            var member = await _memberClient.GetAsync(memberId, cancellationToken);
            if (member == null)
            {
                throw new NullReferenceException();
            }

            // TODO: 狀態檢查

            await _memberClient.RemoveAsync(memberId, cancellationToken);
        }
    }
}
