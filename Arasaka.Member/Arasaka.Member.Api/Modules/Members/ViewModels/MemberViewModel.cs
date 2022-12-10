namespace Arasaka.Member.Api.Modules.Members.ViewModels;

public class MemberViewModel
{
    public long Id { get; set; }

    #region 個人隱私資料

    public DateOnly Birthday { get; set; }
    public string Gender { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    #endregion

    #region 無法辨識個人身分的系統資料

    public string State { get; set; }
    public DateTimeOffset RegisterTime { get; set; }
    public string RegisterFrom { get; set; }
    public DateTimeOffset LastUpdateTime { get; set; }

    #endregion
}
