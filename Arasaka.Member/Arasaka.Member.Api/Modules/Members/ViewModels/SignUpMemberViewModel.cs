﻿using System.ComponentModel.DataAnnotations;

namespace Arasaka.Member.Api.Modules.Members.ViewModels;

public class SignUpMemberViewModel
{
    #region 個人隱私資料

    [Required]
    public DateOnly Birthday { get; set; }
    [Required]
    public string Gender { get; set; }
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