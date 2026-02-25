using EnglishCentralManagement.Dtos.Invoice;
using EnglishCentralManagement.Extensions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EnglishCentralManagement.Documents
{
    public class TuitionReceiptDocument : IDocument
    {
        private readonly TuitionReceiptDto _model;

        public TuitionReceiptDocument(TuitionReceiptDto model)
        {
            _model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A5);
                page.Margin(40);

                page.DefaultTextStyle(x => x
                    .FontSize(12)
                    .FontFamily("Times New Roman"));

                page.Content().Column(col =>
                {
                    col.Spacing(15);

                    BuildHeader(col);
                    BuildTitle(col);
                    BuildContentInfo(col);
                    BuildTable(col);
                    BuildFooter(col);
                });
            });
        }

        private void BuildHeader(ColumnDescriptor col)
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Column(left =>
                {
                    left.Item().Text("TRUNG TÂM NGOẠI NGỮ ABC").Bold().FontSize(12);
                    left.Item().Text("Địa chỉ: 99 Đà Nẵng");
                    left.Item().Text("Điện thoại: 0909 999 999");
                });

                row.RelativeItem().AlignRight().Column(right =>
                {
                    right.Item().Text($"Mã Số Biên Lai: {_model.ReceiptNumber}").SemiBold();
                });
            });

            col.Item().LineHorizontal(1);
        }

        private void BuildTitle(ColumnDescriptor col)
        {
            col.Item().AlignCenter().Text("BIÊN LAI THU HỌC PHÍ")
                .Bold()
                .FontSize(18);

            col.Item().LineHorizontal(1);
        }

        private void BuildContentInfo(ColumnDescriptor col)
        {
            col.Spacing(12);

            BuildDottedField(col, "Học viên:", _model.StudentName);
            BuildDottedField(col, "Lớp:", _model.ClassName);
            BuildDottedField(col, "Địa Chỉ:", _model.Address);
            BuildDottedField(col, "SĐT:", _model.Phone);
        }

        private void BuildTable(ColumnDescriptor col)
        {
            col.Item().PaddingTop(15).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(50);   // STT
                    columns.RelativeColumn();     // Khoản thu
                    columns.ConstantColumn(120);  // Số tiền
                });

                // Header
                table.Header(header =>
                {
                    header.Cell().Border(1).Padding(5).Text("STT").Bold().AlignCenter();
                    header.Cell().Border(1).Padding(5).Text("Nội dung thu").Bold().AlignCenter();
                    header.Cell().Border(1).Padding(5).AlignRight().Text("Số tiền (VNĐ)").Bold().AlignCenter();
                });

                int index = 1;
                foreach (var item in _model.Items)
                {
                    table.Cell().Border(1).Padding(5).Text(index.ToString());
                    table.Cell().Border(1).Padding(5).Text(item.Description);
                    table.Cell().Border(1).Padding(5)
                        .AlignRight()
                        .Text($"{item.Amount:N0}");
                    index++;
                }

                // TOTAL (Merge STT + Khoản thu)
                table.Cell()
                    .ColumnSpan(2)
                    .Border(1)
                    .Padding(5)
                    .AlignRight()
                    .Text("TỔNG")
                    .Bold();

                table.Cell()
                    .Border(1)
                    .Padding(5)
                    .AlignRight()
                    .Text($"{_model.Total:N0}")
                    .Bold();
            });
            col.Item().PaddingTop(10);
            BuildDottedField(col, "Số Tiền Bằng chữ:", NumberToText(_model.Total));
        }

        private void BuildFooter(ColumnDescriptor col)
        {
            col.Item().PaddingTop(30);
            // ===== Dòng ngày tháng bên phải =====
            col.Item().AlignRight().Text(text =>
            {
                text.Span("Đà Nẵng, ").SemiBold();
                text.Span("Ngày ").SemiBold();
                text.Span($"{_model.Date:dd} ");
                text.Span("Tháng ").SemiBold();
                text.Span($"{_model.Date:MM} ");
                text.Span("Năm ").SemiBold();
                text.Span($"{_model.Date:yyyy}");
            });

            col.Item().PaddingTop(20).Row(row =>
            {
                row.RelativeItem().AlignCenter().Column(c =>
                {
                    c.Item().Text("Người nộp tiền").Bold();
                    c.Item().Text("(Ký, ghi rõ họ tên)").Italic();
                });

                row.RelativeItem().AlignCenter().Column(c =>
                {
                    c.Item().Text("Kế toán").Bold();
                    c.Item().Text("(Ký, ghi rõ họ tên)").Italic();
                });

                row.RelativeItem().AlignCenter().Column(c =>
                {
                    c.Item().Text("Người Thu Tiền").Bold();
                    c.Item().Text("(Ký, đóng dấu)").Italic();
                });
            });
        }

        private string NumberToText(decimal input)
        {
            if (input == 0)
                return "Không đồng";

            long number = (long)input;

            string result = ReadNumber(number);

            // Viết hoa chữ cái đầu
            result = char.ToUpper(result[0]) + result.Substring(1);

            return result + " đồng";
        }

        private string ReadNumber(long number)
        {
            if (number == 0)
                return "";

            string result = "";

            long billions = number / 1_000_000_000;
            long millions = (number % 1_000_000_000) / 1_000_000;
            long thousands = (number % 1_000_000) / 1_000;
            long hundreds = number % 1_000;

            if (billions > 0)
                result += ReadThreeDigits(billions) + " tỷ ";

            if (millions > 0)
                result += ReadThreeDigits(millions) + " triệu ";

            if (thousands > 0)
                result += ReadThreeDigits(thousands) + " nghìn ";

            if (hundreds > 0)
                result += ReadThreeDigits(hundreds);

            return result.Trim();
        }

        private string ReadThreeDigits(long number)
        {
            string result = "";

            int tram = (int)(number / 100);
            int chuc = (int)((number % 100) / 10);
            int donvi = (int)(number % 10);

            if (tram > 0)
            {
                result += DocumentExtension.ChuSo[tram] + " trăm ";
            }

            if (chuc > 1)
            {
                result += DocumentExtension.ChuSo[chuc] + " mươi ";

                if (donvi == 1)
                    result += "mốt ";
                else if (donvi == 5)
                    result += "lăm ";
                else if (donvi > 0)
                    result += DocumentExtension.ChuSo[donvi] + " ";
            }
            else if (chuc == 1)
            {
                result += "mười ";

                if (donvi == 5)
                    result += "lăm ";
                else if (donvi > 0)
                    result += DocumentExtension.ChuSo[donvi] + " ";
            }
            else if (chuc == 0 && donvi > 0)
            {
                if (tram > 0)
                    result += "lẻ ";

                if (donvi == 5 && tram > 0)
                    result += "lăm ";
                else
                    result += DocumentExtension.ChuSo[donvi] + " ";
            }

            return result;
        }
        private void BuildDottedField(ColumnDescriptor col, string label, string value, bool isItalic = false)
        {
            col.Item().Row(row =>
            {
                row.ConstantItem(100).Text(label).SemiBold();

                row.RelativeItem().Column(column =>
                {
                    if (isItalic)
                    {
                        column.Item().Text(value).Italic();
                    }
                    else
                    {
                        column.Item().Text(value);
                    }

                    column.Item().LineHorizontal(1)
                        .LineColor(Colors.Black)
                        .LineDashPattern(new float[] { 2, 2 });   // <-- dòng chấm
                });
            });
        }
    }
}
