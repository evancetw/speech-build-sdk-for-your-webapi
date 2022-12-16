using Arasaka.Member.Api.Modules.Members.ViewModels;
using Arasaka.Member.Api.Repositories;
using System.Reflection;
using static Arasaka.Member.Api.Modules.Members.Constants;

namespace Arasaka.Member.Api.Modules.ArasakaSystem;

public static class ArasakaSystemModule
{
    public static string Version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

    public static IEndpointRouteBuilder MapArasakaSystemModuleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/_system/version", () => Version);

        return endpoints;
    }
}
