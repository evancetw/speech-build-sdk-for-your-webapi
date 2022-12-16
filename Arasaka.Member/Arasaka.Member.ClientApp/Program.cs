// See https://aka.ms/new-console-template for more information
using Arasaka.Member.ClientSDK;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Arasaka.Member.ClientApp.Utilities;

Console.WriteLine("Hello, World!");
Console.WriteLine();


using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Default", LogLevel.Debug)
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddConsole();
});

ILogger<MemberClient> logger = loggerFactory.CreateLogger<MemberClient>();

MemberClient memberClient = null;

try
{
    var httpClient = new HttpClient();
    memberClient = new MemberClient("http://localhost:5101/", httpClient, logger);

    // 故意打個錯的網址
    var rsp = await httpClient.GetAsync("http://njrjkwodsifhqwerhhh.com");
    rsp.EnsureSuccessStatusCode();

}
catch (Arasaka.Member.ClientSDK.Exceptions.MemberApiClientException ex)
{
    Console.WriteLine($"SDK 自定義的 exception");
    Console.WriteLine($"SDK 自定義的 exception type: {ex.GetType().Name}");
    Console.WriteLine($"SDK 自定義的 exception message: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"未處理的 exception");
    Console.WriteLine($"未處理的 exception type: {ex.GetType().Name}");
    Console.WriteLine($"未處理的 exception message: {ex.Message}");
}

Console.ReadLine();
return;



await Demo(@"get exist member",
    async () =>
    {
        await DemoGetMember(memberClient, memberId: 91);
    });

long newMemberId = default;

await Demo(@"member signup",
    async () =>
    {
        string newMemberName = Guid.NewGuid().ToString().Substring(0, 6);
        var newMember = await memberClient.SignUpAsync(
            new Arasaka.Member.ClientSDK.Models.MemberSignUpForm
            {
                Name = newMemberName,
                Address = "高雄市",
                Birthday = DateTimeOffset.UtcNow.AddYears(-30),
                Email = $"{newMemberName}@hoiwqeio.com",
                Gender = Constants.Gender.Male,
                PhoneNumber = "0912345678",
                SignUpFrom = "SDK"
            },
            default);

        Console.WriteLine(JsonSerializer.Serialize(newMember, OutputJsonSerializerOptions));
        newMemberId = newMember.Id;
    });

await Demo(@"verify member",
    async () =>
    {
        await memberClient.VerifyAsync(newMemberId, default);

        await DemoGetMember(memberClient, memberId: newMemberId);
    });

await Demo(@"restrict member",
    async () =>
    {
        await memberClient.RestrictAsync(newMemberId, default);

        await DemoGetMember(memberClient, memberId: newMemberId);
    });

await Demo(@"allow member",
    async () =>
    {
        await memberClient.AllowAsync(newMemberId, default);

        await DemoGetMember(memberClient, memberId: newMemberId);
    });

await Demo(@"restrict member again",
    async () =>
    {
        await memberClient.RestrictAsync(newMemberId, default);

        await DemoGetMember(memberClient, memberId: newMemberId);
    });

await Demo(@"ban member",
    async () =>
    {
        await memberClient.BanAsync(newMemberId, default);

        await DemoGetMember(memberClient, memberId: newMemberId);
    });

await Demo(@"permit member",
    async () =>
    {
        await memberClient.PermitAsync(newMemberId, default);

        await DemoGetMember(memberClient, memberId: newMemberId);
    });

await Demo(@"ban member again",
    async () =>
    {
        await memberClient.BanAsync(newMemberId, default);

        await DemoGetMember(memberClient, memberId: newMemberId);
    });

await Demo(@"remove member",
    async () =>
    {
        await memberClient.RemoveAsync(newMemberId, default);

        await DemoGetMember(memberClient, memberId: newMemberId);
    });







Console.WriteLine();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();



// --------- 


static async Task Demo(string demoName, Func<Task> action)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(new String('#', 60));
    Console.WriteLine($"Start to demo: {demoName}");
    Console.WriteLine();
    Console.ResetColor();

    try
    {
        await action();
    }
    catch (global::System.Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(ex.ToString());
        Console.ResetColor();
    }

    Console.WriteLine();
    Console.WriteLine();
}


static async Task DemoGetMember(MemberClient memberClient, long memberId)
{
    var memberInfo = await memberClient.GetAsync(memberId, default);
    Console.WriteLine(JsonSerializer.Serialize(memberInfo, OutputJsonSerializerOptions));
}
