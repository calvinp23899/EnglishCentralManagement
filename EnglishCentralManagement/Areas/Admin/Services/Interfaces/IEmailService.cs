using EnglishCentralManagement.Dtos.Email;
using EnglishCentralManagement.Models.Enum;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(GmailDto gmailData, TemplateEmailEnum type);
    }
}
