using Arasaka.Member.Api.Repositories;
using Arasaka.Member.Api.ViewModels;

namespace Arasaka.Member.Api;

internal static class Extensions
{
    internal static MemberEntity ConvertToEntity(this RegisterMemberViewModel registerMemberViewModel)
    {
        return new MemberEntity
        {
            Address = registerMemberViewModel.Address,
            Birthday = registerMemberViewModel.Birthday,
            Email = registerMemberViewModel.Email,
            Gender = registerMemberViewModel.Gender,
            Name = registerMemberViewModel.Name,
            PhoneNumber = registerMemberViewModel.PhoneNumber,
            RegisterFrom = registerMemberViewModel.RegisterFrom,
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

    public static T RandomEnum<T>(Random? random = null) where T : struct, IConvertible
    {
        if (!typeof(T).IsEnum) { throw new Exception("random enum variable is not an enum"); }

        random = random ?? new Random();

        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(random.Next(values.Length));
    }
}
