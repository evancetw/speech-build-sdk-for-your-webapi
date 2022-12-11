using Arasaka.Member.Api.Modules.Members.Models;
using Arasaka.Member.Api.Modules.Members.ViewModels;
using static Arasaka.Member.Api.Modules.Members.Constants;

namespace Arasaka.Member.Api.Modules.Members;

public static class Extensions
{
    internal static MemberEntity ConvertToEntity(this SignUpMemberViewModel registerMemberViewModel)
    {
        return new MemberEntity
        {
            Address = registerMemberViewModel.Address,
            Birthday = DateOnly.FromDateTime(registerMemberViewModel.Birthday.UtcDateTime),
            Email = registerMemberViewModel.Email,
            Gender = registerMemberViewModel.Gender.ToString(),
            Name = registerMemberViewModel.Name,
            PhoneNumber = registerMemberViewModel.PhoneNumber,
            RegisterFrom = registerMemberViewModel.SignUpFrom,
        };
    }

    internal static void AssignFieldsTo(this UpdateMemberViewModel updateMemberViewModel, MemberEntity updatingMemberEntity)
    {
        updatingMemberEntity.Address = updateMemberViewModel.Address ?? updatingMemberEntity.Address;
        updatingMemberEntity.Email = updateMemberViewModel.Email ?? updatingMemberEntity.Email;
        updatingMemberEntity.Gender = updateMemberViewModel.Gender.HasValue ? updateMemberViewModel.Gender.Value.ToString() : updatingMemberEntity.Gender;
        updatingMemberEntity.Name = updateMemberViewModel.Name ?? updatingMemberEntity.Name;
        updatingMemberEntity.PhoneNumber = updateMemberViewModel.PhoneNumber ?? updatingMemberEntity.PhoneNumber;
    }

    internal static MemberViewModel ConvertToViewModel(this MemberEntity memberEntity)
    {
        return new MemberViewModel
        {
            Id = memberEntity.Id,

            Address = memberEntity.Address,
            Birthday = memberEntity.Birthday.ToDateTime(new TimeOnly(0, 0), DateTimeKind.Utc),
            Email = memberEntity.Email,
            Gender = Enum.Parse<Gender>(memberEntity.Gender),
            Name = memberEntity.Name,
            PhoneNumber = memberEntity.PhoneNumber,

            RegisterFrom = memberEntity.RegisterFrom,
            RegisterTime = memberEntity.RegisterTime,
            State = memberEntity.State,
            LastUpdateTime = memberEntity.LastUpdateTime,
        };
    }
}
