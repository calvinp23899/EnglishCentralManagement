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