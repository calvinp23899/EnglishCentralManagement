using ClosedXML.Excel;
using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class ExcelService : IExcelService
    {
        public ExcelService()
        {
        }

        public byte[] ExportStaffExcel(List<StaffExcelDto> staffs)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Staff");

            // Header
            ws.Cell(1, 1).Value = "Full Name";
            ws.Cell(1, 2).Value = "Email";
            ws.Cell(1, 3).Value = "Contract Type";
            ws.Cell(1, 4).Value = "Payment Card";
            ws.Cell(1, 5).Value = "Bank Card";
            ws.Cell(1, 6).Value = "Hourly Rate";
            ws.Cell(1, 7).Value = "Working Hour";
            ws.Cell(1, 8).Value = "Monthly Salary";

            // 👇 Cột J (cột 10)
            ws.Cell(1, 10).Value = "Total Monthly Salary";

            ws.Range(1, 1, 1, 10).Style.Font.Bold = true;

            // Data
            for (int i = 0; i < staffs.Count; i++)
            {
                ws.Cell(i + 2, 1).Value = staffs[i].FullName;
                ws.Cell(i + 2, 2).Value = staffs[i].Email;
                ws.Cell(i + 2, 3).Value = staffs[i].ContractType;
                ws.Cell(i + 2, 4).Value = staffs[i].PaymentCard;
                ws.Cell(i + 2, 5).Value = staffs[i].BankCard;
                ws.Cell(i + 2, 6).Value = staffs[i].HourlyRate;
                ws.Cell(i + 2, 7).Value = staffs[i].WorkingHour;
                ws.Cell(i + 2, 8).Value = staffs[i].MonthlySalary;
            }

            // Format tiền
            ws.Column(6).Style.NumberFormat.Format = "#,##0";
            ws.Column(8).Style.NumberFormat.Format = "#,##0";
            ws.Column(10).Style.NumberFormat.Format = "#,##0";

            // tổng MonthlySalary
            int lastRow = staffs.Count + 1;

            ws.Cell(2, 10).FormulaA1 = $"SUM(H2:H{lastRow})";

            // Làm nổi bật ô tổng
            ws.Cell(2, 10).Style.Font.Bold = true;
            ws.Cell(2, 10).Style.Fill.BackgroundColor = XLColor.LightYellow;

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }
    }
}
