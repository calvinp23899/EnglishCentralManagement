let paymentSchedule = null;
function openPaymentModal(paymentId) {
    //document.getElementById("paymentModal").classList.add("show");
    paymentSchedule = paymentId
    const URL = "/Admin/Payment/EditPayment";
    // mở modal
    $("#paymentModal").addClass("show");
   
}

function closePaymentModal() {
    document.getElementById("paymentModal").classList.remove("show");
}

function savePayment() {
    const URL = "/Admin/Payment/EditPayment";
    const btn = $(".btn.primary");

    btn.prop("disabled", true);
    $.ajax({
        url: URL,
        type: "POST",
        data: {
            id: paymentSchedule,

            PaidAt: $("#PaidAt").val(),
            PaidAmount: $("#PaidAmount").val(),
            PaymentMethod: $("#PaymentMethod").val(),

            __RequestVerificationToken:
                $('#antiForgeryForm input[name="__RequestVerificationToken"]').val()
        },
        success: function () {
            showToast("Edit Payment Successfully", "success");
            closePaymentModal();
            location.reload(); // nếu cần refresh
        },
        error: function () {
            showToast("Action failed", "error");
        },
        complete: function () {
            btn.prop("disabled", false);
        }
    });
}

function DownloadInvoice(id) {
    const URL = "/Admin/Enrollment/DownloadInvoice";
    const token = $('#antiForgeryForm input[name="__RequestVerificationToken"]').val();
    const btn = $(".btn.primary");

    btn.prop("disabled", true);
    $.ajax({
        url: URL,
        type: "POST",
        data: {
            id: id
        },
        headers: {
            "RequestVerificationToken": token
        },
        xhrFields: {
            responseType: 'blob'  
        },
        success: function (data, status, xhr) {

            const blob = new Blob([data], { type: "application/pdf" });
            const url = window.URL.createObjectURL(blob);

            let fileName = "download.pdf";

            const disposition = xhr.getResponseHeader("Content-Disposition");

            if (disposition) {

                // Ưu tiên filename* (UTF-8 chuẩn)
                const fileNameStarMatch = disposition.match(/filename\*=UTF-8''([^;]+)/);

                if (fileNameStarMatch && fileNameStarMatch.length > 1) {
                    fileName = decodeURIComponent(fileNameStarMatch[1]);
                } else {
                    // fallback filename thường
                    const fileNameMatch = disposition.match(/filename="?([^"]+)"?/);
                    if (fileNameMatch && fileNameMatch.length > 1) {
                        fileName = fileNameMatch[1];
                    }
                }
            }

            const a = document.createElement("a");
            a.href = url;
            a.download = fileName;
            document.body.appendChild(a);
            a.click();

            a.remove();
            window.URL.revokeObjectURL(url);
        },
        error: function () {
            showToast("Download failed", "error");
        }
    });
}


