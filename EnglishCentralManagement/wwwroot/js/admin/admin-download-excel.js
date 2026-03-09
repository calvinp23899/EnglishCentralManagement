function downloadStaffExcel(btn) {
    const url = '/Admin/Teacher/DownloadStaffExcel';
    showToast("Preparing excel download...", "progress");

    // disable link
    btn.disabled = true;
    btn.innerText = "Downloading...";
    setTimeout(() => {
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
                showToast("Excel downloaded successfully", "success");
                btn.disabled = false;
                btn.innerText = "Download Excel";
            },
            error: function (xhr) {

                if (xhr.response) {
                    const reader = new FileReader();
                    reader.onload = function () {
                        try {
                            const err = JSON.parse(reader.result);
                            showToast(err.message, "error");
                        } catch {
                            showToast("Download failed", "error");
                        }
                    };
                    reader.readAsText(xhr.response);
                } else {
                    showToast("Download failed", "error");
                }
            }
        })

    }, 2000);

}