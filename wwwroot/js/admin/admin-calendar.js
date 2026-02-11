document.addEventListener("DOMContentLoaded", function () {

    const panel = document.querySelector(".event-detail-panel");

    document.querySelectorAll(".event").forEach(e => {
        e.addEventListener("click", function () {
            panel.classList.add("active");
        });
    });

    document.querySelector(".close-btn").addEventListener("click", function () {
        panel.classList.remove("active");
    });

});