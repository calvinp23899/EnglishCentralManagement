$(document).ready(function () {

    // Smooth scroll navbar
    $(".nav-link").click(function (e) {

        var target = $(this).attr("href");

        if (target && target.startsWith("#")) {

            e.preventDefault();

            $("html, body").animate({
                scrollTop: $(target).offset().top - 70
            }, 600);

        }

    });

    // Show / Hide back to top
    $(window).scroll(function () {

        if ($(this).scrollTop() > 30) {
            $("#backToTop").fadeIn();
        } else {
            $("#backToTop").fadeOut();
        }

    });

    // Click back to top
    $("#backToTop").click(function () {

        $("html, body").animate({
            scrollTop: 0
        }, 600);

        return false;

    });

});

function submitRegisterForm() {
    const data = getRegisterFormData();

    if (!validateRegisterForm(data))
        return;
    console.log("Call API");
    $.ajax({
        url: "/Home/RegisterStudent",  
        method: "POST",
        data: data,
        headers: {
            'RequestVerificationToken':
                $('#antiForgeryForm input[name="__RequestVerificationToken"]').val()
        },
        beforeSend: function () {
            onRegisterBeforeSend();
        },
        success: function (res) {
            onRegisterSuccess(res);
        },
        error: function (err) {
            console.log("status: ", err.status);
            if (err.status === 429 || err.status === 503) {
                alert("Server đạng nhận quá nhiều yêu cầu, vui lòng thử lại sau.");
            }else{
                onRegisterError(err);
            }
        },
        complete: function () {
            onRegisterComplete();
        }
    });
}

function validateRegisterForm(data) {
    if (!data.fullName) {
        alert("Vui lòng nhập họ và tên.");
        return false;
    }
    if (!data.phone) {
        alert("Vui lòng nhập số điện thoại.");
        return false;
    }
    if (!data.course || data.course === "Khóa học *") {
        alert("Vui lòng chọn khóa học.");
        return false;
    }
    return true;
}

function getRegisterFormData() {
    const data = {
        fullName: $("#registerModal input[placeholder='Họ Và Tên *']").val(),
        phone: $("#registerModal input[placeholder='Điện thoại *']").val(),
        email: $("#registerModal input[type='email']").val(),
        learnType: $("#registerModal select").eq(0).val(),
        address: $("#registerModal select").eq(1).val(),
        course: $("#registerModal select").eq(2).val(),
    };
    return data;
}

function onRegisterBeforeSend() {
    $(".register-btn").prop("disabled", true).text("Đang gửi...");
}

function onRegisterSuccess(res) {
    alert("Đăng ký tư vấn thành công!");
    $("#registerModal").modal("hide");
    $("#registerModal form")[0].reset();
}

function onRegisterError(err) {
    alert("Có lỗi xảy ra, vui lòng thử lại.");
    console.error(err);
}

function onRegisterComplete() {
    $(".register-btn").prop("disabled", false).text("TƯ VẤN NGAY");
}