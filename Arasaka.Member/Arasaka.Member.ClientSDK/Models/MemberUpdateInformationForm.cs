using static Arasaka.Member.ClientSDK.Constants;

namespace Arasaka.Member.ClientSDK.Models
{
    public class MemberUpdateInformationForm
    {
        public Gender Gender { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
