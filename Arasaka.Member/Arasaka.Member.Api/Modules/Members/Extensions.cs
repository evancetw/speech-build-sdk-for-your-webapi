using Arasaka.Member.Api.Modules.Members.Models;
using Arasaka.Member.Api.Modules.Members.ViewModels;
using static Arasaka.Member.Api.Utities.CommonExtension;

namespace Arasaka.Member.Api.Modules.Members;

public static class Extensions
{
    internal static MemberEntity ConvertToEntity(this SignUpMemberViewModel registerMemberViewModel)
    {
        return new MemberEntity
        {
            Address = registerMemberViewModel.Address,
            Birthday = registerMemberViewModel.Birthday,
            Email = registerMemberViewModel.Email,
            Gender = registerMemberViewModel.Gender,
            Name = registerMemberViewModel.Name,
            PhoneNumber = registerMemberViewModel.PhoneNumber,
            RegisterFrom = registerMemberViewModel.SignUpFrom,
        };
    }

    internal static void AssignFieldsTo(this UpdateMemberViewModel updateMemberViewModel, MemberEntity updatingMemberEntity)
    {
        updatingMemberEntity.Address = updateMemberViewModel.Address ?? updatingMemberEntity.Address;
        updatingMemberEntity.Email = updateMemberViewModel.Email ?? updatingMemberEntity.Email;
        updatingMemberEntity.Gender = updateMemberViewModel.Gender ?? updatingMemberEntity.Gender;
        updatingMemberEntity.Name = updateMemberViewModel.Name ?? updatingMemberEntity.Name;
        updatingMemberEntity.PhoneNumber = updateMemberViewModel.PhoneNumber ?? updatingMemberEntity.PhoneNumber;
    }

    internal static MemberViewModel ConvertToViewModel(this MemberEntity memberEntity)
    {
        return new MemberViewModel
        {
            Id = memberEntity.Id,

            Address = memberEntity.Address,
            Birthday = memberEntity.Birthday,
            Email = memberEntity.Email,
            Gender = memberEntity.Gender,
            Name = memberEntity.Name,
            PhoneNumber = memberEntity.PhoneNumber,

            RegisterFrom = memberEntity.RegisterFrom,
            RegisterTime = memberEntity.RegisterTime,
            State = memberEntity.State,
            LastUpdateTime = memberEntity.LastUpdateTime,
        };
    }
}
