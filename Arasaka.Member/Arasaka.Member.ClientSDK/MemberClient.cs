using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Arasaka.Member.ClientSDK.Models;
using Arasaka.Member.ClientSDK.Utities;

namespace Arasaka.Member.ClientSDK
{
    /// <summary>
    /// Member API 客戶端
    /// </summary>
    public class MemberClient : IMemberClient
    {
        private string _baseUrl = "";
        private HttpClient _httpClient;

        private static readonly Version _version = new Version(
            Assembly.GetAssembly(typeof(MemberClient)).GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion);

        private Version GetApiVersion()
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append($"/_system/version");

            var response = _httpClient.GetAsync(urlBuilder.ToString()).GetAwaiter().GetResult();
            var version = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return new Version(version);
        }

        public MemberClient(string baseUrl, HttpClient httpClient)
        {
            _baseUrl = baseUrl;
            _httpClient = httpClient;

            ApiVersion = GetApiVersion();

            Console.WriteLine($@"{new String('#', 60)}
  Client SDK Version: {Version}
  API Version       : {ApiVersion}");

            Console.WriteLine();
        }

        public string BaseUrl
        {
            get { return _baseUrl; }
            set { _baseUrl = value; }
        }

        public Version Version => _version;

        public Version ApiVersion { get; private set; }


        private async Task SendAsync(
            HttpMethod httpMethod,
            string url,
            Func<HttpResponseMessage, Task> handleResponse,
            Func<Task<StringContent>> generateJsonStringContent = null,
            CancellationToken? cancellationToken = null)
        {
            var ct = cancellationToken ?? CancellationToken.None;

            var client = _httpClient;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = httpMethod;
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

                    if (generateJsonStringContent != null)
                    {
                        request.Content = await generateJsonStringContent.Invoke().ConfigureAwait(false);
                        request.Content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                    }

                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
                    try
                    {
                        if (handleResponse != null)
                            await handleResponse.Invoke(response).ConfigureAwait(false);
                    }
                    finally
                    {
                        if (response != null)
                            response.Dispose();
                    }
                }
            }
            finally
            {
            }
        }

        public async Task AllowAsync(long memberId, CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append($"/members/{memberId}:allow");

            await this.SendAsync(
                HttpMethod.Post, urlBuilder.ToString(),
                handleResponse: (response) =>
                {
                    response.EnsureSuccessStatusCode();
                    return Task.CompletedTask;
                },
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task BanAsync(long memberId, CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append($"/members/{memberId}:ban");

            await this.SendAsync(
                HttpMethod.Post, urlBuilder.ToString(),
                handleResponse: (response) =>
                {
                    response.EnsureSuccessStatusCode();
                    return Task.CompletedTask;
                },
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<MemberInformation> GetAsync(long memberId, CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append($"/members/{memberId}");

            var client = _httpClient;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Get;

                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        response.EnsureSuccessStatusCode();

                        return await JsonSerializerHelper.DeserializeAsync<MemberInformation>(
                            await response.Content.ReadAsStreamAsync().ConfigureAwait(false)).ConfigureAwait(false);

                        //else
                        //if (status_ != "200" && status_ != "204")
                        //{
                        //    var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        //    throw new ApiException("The HTTP status code of the response was not expected (" + (int)response.StatusCode + ").", (int)response.StatusCode, responseData, headers_, null);
                        //}
                    }
                    finally
                    {
                        if (response != null)
                            response.Dispose();
                    }
                }
            }
            finally
            {
            }
        }

        public async Task PermitAsync(long memberId, CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append($"/members/{memberId}:permit");

            await this.SendAsync(
                HttpMethod.Post, urlBuilder.ToString(),
                handleResponse: (response) =>
                {
                    response.EnsureSuccessStatusCode();
                    return Task.CompletedTask;
                },
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task RemoveAsync(long memberId, CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append($"/members/{memberId}:remove");

            await this.SendAsync(
                HttpMethod.Delete, urlBuilder.ToString(),
                handleResponse: (response) =>
                {
                    response.EnsureSuccessStatusCode();
                    return Task.CompletedTask;
                },
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task RestrictAsync(long memberId, CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append($"/members/{memberId}:restrict");

            await this.SendAsync(
                HttpMethod.Post, urlBuilder.ToString(),
                handleResponse: (response) =>
                {
                    response.EnsureSuccessStatusCode();
                    return Task.CompletedTask;
                },
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<MemberInformation> SignUpAsync(MemberSignUpForm memberSignUpForm, CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append($"/members:signup");

            MemberInformation memberInformation = null;

            await this.SendAsync(
                HttpMethod.Post, urlBuilder.ToString(),
                generateJsonStringContent: () =>
                {
                    return Task.FromResult(new StringContent(JsonSerializerHelper.Serialize(memberSignUpForm)));
                },
                handleResponse: async (response) =>
                {
                    response.EnsureSuccessStatusCode();

                    memberInformation = await JsonSerializerHelper.DeserializeAsync<MemberInformation>(
                        await response.Content.ReadAsStreamAsync().ConfigureAwait(false)).ConfigureAwait(false);
                },
                cancellationToken: cancellationToken).ConfigureAwait(false);

            if (memberInformation == null)
                throw new Exception();

            return memberInformation;
        }

        public async Task VerifyAsync(long memberId, CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append($"/members/{memberId}:verify");

            await this.SendAsync(
                HttpMethod.Post, urlBuilder.ToString(),
                handleResponse: (response) =>
                {
                    response.EnsureSuccessStatusCode();
                    return Task.CompletedTask;
                },
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<MemberInformation>> ListAsync(ListMembersFilter listMembersFilter, CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append($"/members:list?pageSize={listMembersFilter.PageSize}&pageNumber={listMembersFilter.PageNumber}");

            IEnumerable<MemberInformation> memberInformations = null;

            await this.SendAsync(
                HttpMethod.Get, urlBuilder.ToString(),
                handleResponse: async (response) =>
                {
                    response.EnsureSuccessStatusCode();

                    memberInformations = await JsonSerializerHelper.DeserializeAsync<IEnumerable<MemberInformation>>(
                        await response.Content.ReadAsStreamAsync().ConfigureAwait(false)).ConfigureAwait(false);
                },
                cancellationToken: cancellationToken).ConfigureAwait(false);

            if (memberInformations == null)
                throw new Exception();

            return memberInformations;
        }
    }
}
