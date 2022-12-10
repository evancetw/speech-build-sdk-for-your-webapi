namespace Arasaka.Member.Api.Utities;

internal static class CommonExtension
{
    public static T RandomEnum<T>(Random? random = null) where T : struct, IConvertible
    {
        if (!typeof(T).IsEnum) { throw new Exception("random enum variable is not an enum"); }

        random = random ?? new Random();

        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(random.Next(values.Length));
    }
}
