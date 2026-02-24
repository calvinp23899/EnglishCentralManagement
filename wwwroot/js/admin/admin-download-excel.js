function downloadStaffExcel() {
    const url = '/Admin/Teacher/DownloadStaffExcel';
    $.ajax({
        url: url,
        type: 'POST',
        xhrFields: {
            responseType: 'blob' 
        },
        success: function (data) {
            var blob = new Blob([data], {
                type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
            });

            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = "Staff_List.xlsx";
            link.click();

            window.URL.revokeObjectURL(link.href);
        },
        error: function () {
            showToast("Failed to download", "error");
        }
    })
}