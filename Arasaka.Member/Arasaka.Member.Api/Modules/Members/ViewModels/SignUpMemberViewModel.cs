using System.ComponentModel.DataAnnotations;
using static Arasaka.Member.Api.Modules.Members.Constants;

namespace Arasaka.Member.Api.Modules.Members.ViewModels;

public class SignUpMemberViewModel
{
    #region 個人隱私資料

    /// <summary>
    /// yyyy/MM/dd
    /// </summary>
    [Required]
    public DateTimeOffset Birthday { get; set; }
    [Required]
    public Gender Gender { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    public string Email { get; set; }

    #endregion

    [Required]
    public string SignUpFrom { get; set; }
}
