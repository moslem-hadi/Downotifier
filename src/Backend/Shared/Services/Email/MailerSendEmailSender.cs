using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;


namespace Shared.Services.Email
{
    /// <summary>
    /// Add to DI using EmailSenderRegister.AddMailerSend
    /// </summary>
    public class MailerSendEmailSender : IEmailSender
    {
        private readonly HttpClient _httpClient;
        private readonly MailerSendOptions _options;

        public MailerSendEmailSender(HttpClient httpClient, IOptions<MailerSendOptions> options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrWhiteSpace(_options.ApiToken))
                throw new ArgumentException("Api token is null or empty");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _options.ApiToken);
        }

        public async Task SendMailAsync(Recipient to, string? subject = null, string? text = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_options.SenderEmail))
                throw new ArgumentException("Sender email address is null or empty");

            var from = new Recipient(_options.SenderEmail, _options.SenderName);

            var requestBody = new
            {
                from,
                to = new List<Recipient> { to },
                subject,
                text
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var response = await _httpClient.PostAsJsonAsync("email", requestBody, jsonOptions, cancellationToken);

            response.EnsureSuccessStatusCode();
        }
    }
}
