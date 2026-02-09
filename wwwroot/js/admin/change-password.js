function openPasswordModal() {
    $("#passwordModal").addClass("show");
}

function closePasswordModal() {
    document.getElementById("passwordModal").classList.remove("show");
    // 1. Clear input
    document.getElementById("changePass").value = '';
    document.getElementById("newPassConfirm").value = '';

    // 2. Reset type về password (phòng trường hợp đã show)
    document.getElementById("changePass").type = 'password';
    document.getElementById("newPassConfirm").type = 'password';

    // 3. Uncheck show password checkbox
    const checkbox = document.getElementById("showPassword");
    if (checkbox) checkbox.checked = false;
}

function savePassword() {
    if (!validateConfirmPassword()) {
        showToast("Confirm password does not match, please try again", "error");
        return; 
    }
    const URL = "/Admin/Profile/ChangePassword";
    const btn = $(".btn.primary");

    btn.prop("disabled", true);
    $.ajax({
        url: URL,
        type: "POST",
        data: {
            newPassword: $("#changePass").val(),
            __RequestVerificationToken:
                $('#antiForgeryForm input[name="__RequestVerificationToken"]').val()
        },
        success: function () {
            closePasswordModal();
            showToast("Change Password Successfully", "success");
        },
        error: function () {
            showToast("Action failed", "error");
        },
        complete: function () {
            btn.prop("disabled", false);
        }
    });
}

function showPassword() {
    var x = document.getElementById("changePass");
    var y = document.getElementById("newPassConfirm");
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }

    if (y.type === "password") {
        y.type = "text";
    } else {
        y.type = "password";
    }
}

function validateConfirmPassword() {
    const password = document.getElementById('changePass').value;
    const confirm = document.getElementById('newPassConfirm').value;
    const confirmInput = document.getElementById('newPassConfirm');

    if (!password || !confirm) {
        return false;
    }

    if (password !== confirm) {
        return false;
    }
    return true;
}
