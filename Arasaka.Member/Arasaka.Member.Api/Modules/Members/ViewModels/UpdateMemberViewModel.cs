using static Arasaka.Member.Api.Modules.Members.Constants;

namespace Arasaka.Member.Api.Modules.Members.ViewModels;

public class UpdateMemberViewModel
{
    public Gender? Gender { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}
