using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Arasaka.Member.ClientSDK.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static ServiceCollection AddMemberClient(
            this ServiceCollection service,
            Action<AddMemberClientOptions> setOption = null)
        {
            var option = new AddMemberClientOptions();
            setOption?.Invoke(option);

            service.AddSingleton<MemberClient>(sp =>
            {
                return new MemberClient(
                    option.BaseUrl,
                    sp.GetRequiredService<HttpClient>(),
                    sp.GetRequiredService<ILogger<MemberClient>>(),
                    option.ClientOptions
                    );
            });

            return service;
        }
    }
}
