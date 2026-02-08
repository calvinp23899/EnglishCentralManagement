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
