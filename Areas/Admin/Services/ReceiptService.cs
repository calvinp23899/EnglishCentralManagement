using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Documents;
using EnglishCentralManagement.Dtos.Invoice;
using QuestPDF.Fluent;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class ReceiptService : IReceiptService
    {
        public ReceiptService()
        {
        }

        public byte[] GenerateTuitionReceipt(TuitionReceiptDto model)
        {
            var document = new TuitionReceiptDocument(model);
            return document.GeneratePdf();
        }
    }
}
