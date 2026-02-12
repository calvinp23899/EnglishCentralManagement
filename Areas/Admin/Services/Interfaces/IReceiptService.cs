using EnglishCentralManagement.Dtos.Invoice;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IReceiptService
    {
        byte[] GenerateTuitionReceipt(TuitionReceiptDto model);
    }
}
