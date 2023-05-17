using Shared.Enums;
using System.Net.Mime;
using System.Text;

namespace Shared.Helper
{
    public static class HttpHelper
    {

        //TODO: Use DI for httpClient
        public static async Task<string> Request(ApiMethod method, string uri,string? jsonBody, Dictionary<string, string>? headers)
        {
            //TODO: We can use Polly and retry policy
            using var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = method == ApiMethod.Get ? HttpMethod.Get : HttpMethod.Post,
                RequestUri = new Uri(uri),
            };
            if (!string.IsNullOrEmpty(jsonBody))
                request.Content = new StringContent(jsonBody, Encoding.UTF8);

            if (headers != null)
                foreach (var header in headers)
                    request.Headers.Add(header.Key, header.Value);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
