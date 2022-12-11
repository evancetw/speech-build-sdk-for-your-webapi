using System;
using static Arasaka.Member.ClientSDK.Constants;

namespace Arasaka.Member.ClientSDK.Models
{

    public class MemberSignUpForm
    {
        #region 個人隱私資料

        public DateTimeOffset Birthday { get; set; }
        public Gender Gender { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        #endregion

        public string SignUpFrom { get; set; }
    }
}