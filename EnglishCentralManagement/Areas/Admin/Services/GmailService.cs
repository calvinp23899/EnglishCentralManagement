using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos.Email;
using EnglishCentralManagement.Helpers;
using EnglishCentralManagement.Models.Constants;
using EnglishCentralManagement.Models.Enum;
using EnglishCentralManagement.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Globalization;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class GmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private const string EmailSection = "GoogleOauth2";
        private readonly EmailTemplateService _templateService;

        public GmailService(IConfiguration config, EmailTemplateService templateService)
        {
            _config = config;
            _templateService = templateService;
        }

        public async Task SendAsync(GmailDto data, TemplateEmailEnum type)
        {
            var gmail = _config.GetSection(EmailSection);
            // 1. Lấy Access Token từ Refresh Token
            var credential = new UserCredential(
                new GoogleAuthorizationCodeFlow(
                    new GoogleAuthorizationCodeFlow.Initializer
                    {
                        ClientSecrets = new ClientSecrets
                        {
                            ClientId = gmail["ClientId"],
                            ClientSecret = gmail["SecretKey"]
                        },
                        Scopes = new[] { gmail["Scope"] }
                    }),
                "user",
                new TokenResponse { RefreshToken = gmail["RefreshToken"] }
            );

            await credential.RefreshTokenAsync(CancellationToken.None);

            // 2. Build email
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("EnglishCentral", gmail["SenderEmail"]));
            message.To.Add(new MailboxAddress("", gmail["SenderEmail"]));
            message.Subject = data.Subject;
            string html = await GetHtmlType(data, type);
            message.Body = new TextPart("html") { Text = html };

            // 3. Gửi qua OAuth2
            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(
                new SaslMechanismOAuth2(gmail["SenderEmail"], credential.Token.AccessToken)
            );
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        private async Task<string> GetHtmlType(GmailDto data, TemplateEmailEnum type)
        {
            ValidateTemplateData(type, data);
            var result = type switch
            {
                TemplateEmailEnum.Invoice => await _templateService.RenderAsync(TemplateEmailEnum.Invoice.GetDisplayEnumName(), new Dictionary<string, string>
                {
                    { "InvoiceId", data.InvoiceId },
                    { "CustomerName", data.CustomerName },
                    { "CustomerEmail", data.CustomerEmail },
                    { "ProductName", data.ProductName },
                    { "ProductDescription", data.ProductDescription },
                    { "Amount", string.Format(new CultureInfo("vi-VN"), "{0:N0} ₫", data.ProductAmount) },
                    { "PaymentDate", data.PaymentDate?.ToString("dd/MM/yyyy") },
                    { "PaymentMethod", data.PaymentMethod },
                    { "SupportEmail", CommonConstant.SupportEmailCompany },
                    { "Year", DateTime.Now.Year.ToString() }
                }),

                TemplateEmailEnum.InterviewPass => await _templateService.RenderAsync(TemplateEmailEnum.InterviewPass.GetDisplayEnumName(), new Dictionary<string, string>
                {
                    { "CandidateName", data.CustomerName },
                    { "Position", data.Position },
                    { "Department", data.Department },
                    { "HRCompany", CommonConstant.HrCompanyName },
                    { "Year", DateTime.Now.Year.ToString() }
                }),

                TemplateEmailEnum.InterviewFail => await _templateService.RenderAsync(TemplateEmailEnum.InterviewFail.GetDisplayEnumName(), new Dictionary<string, string>
                {
                    { "CandidateName", data.CustomerName },
                    { "Position", data.Position },
                    { "HRCompany", CommonConstant.HrCompanyName },
                    { "Year", DateTime.Now.Year.ToString() }
                }),

                _ => throw new ArgumentOutOfRangeException(nameof(type), "Template không tồn tại")
            };

            return result;
        }
        private void ValidateTemplateData(TemplateEmailEnum type, dynamic data)
        {
            switch (type)
            {
                case TemplateEmailEnum.Invoice:
                    Require(data.InvoiceId, nameof(data.InvoiceId));
                    Require(data.CustomerName, nameof(data.CustomerName));
                    Require(data.CustomerEmail, nameof(data.CustomerEmail));
                    Require(data.ProductName, nameof(data.ProductName));
                    Require(data.ProductAmount, nameof(data.ProductAmount));
                    break;

                case TemplateEmailEnum.InterviewPass:
                    Require(data.CustomerName, nameof(data.CustomerName));
                    Require(data.Position, nameof(data.Position));
                    Require(data.Department, nameof(data.Department));
                    break;

                case TemplateEmailEnum.InterviewFail:
                    Require(data.CustomerName, nameof(data.CustomerName));
                    Require(data.Position, nameof(data.Position));
                    break;

                default:
                    throw new ArgumentException($"Unsupported template type: {type}");
            }
        }

        private void Require(object value, string fieldName)
        {
            if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
            {
                throw new ArgumentException($"{fieldName} is required");
            }
        }
    }
}
