using System;
using static Arasaka.Member.ClientSDK.Constants;

namespace Arasaka.Member.ClientSDK.Models
{
    public class MemberInformation
    {
        public long Id { get; set; }

        #region 個人隱私資料

        public DateTimeOffset Birthday { get; set; }
        public Gender Gender { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        #endregion

        #region 無法辨識個人身分的系統資料

        public MemberState State { get; set; }
        public DateTimeOffset RegisterTime { get; set; }
        public string RegisterFrom { get; set; }
        public DateTimeOffset LastUpdateTime { get; set; }

        #endregion
    }
}
