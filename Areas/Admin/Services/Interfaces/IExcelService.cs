using EnglishCentralManagement.Dtos;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IExcelService
    {
        byte[] ExportStaffExcel(List<StaffExcelDto> staffs);
    }
}
