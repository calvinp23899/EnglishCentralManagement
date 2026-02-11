let currentDate = new Date();

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
    renderMiniCalendar();
    updateMainHeader();

});

function openEventCalendarModal() {
    $("#eventCalendarModal").addClass("active");
}

function closeEventModal() {
    $("#eventCalendarModal").removeClass("active");
}


function renderMiniCalendar() {
    currentDate.toLocaleDateString("en-US", {
        month: "long",
        year: "numeric"
    });
    const year = currentDate.getFullYear();
    const month = currentDate.getMonth();

    const daysInMonth = getDaysInMonth(year, month);

    const grid = document.getElementById("miniGrid");
    grid.innerHTML = "";

    // Day names
    const dayNames = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

    dayNames.forEach(d => {
        grid.innerHTML += `<div class="day-name">${d}</div>`;
    });

    // First day of month
    const firstDay = new Date(year, month, 1).getDay();

    // Add empty slots
    for (let i = 0; i < firstDay; i++) {
        grid.innerHTML += `<div></div>`;
    }

    // Add days
    for (let i = 1; i <= daysInMonth; i++) {

        const isToday =
            i === new Date().getDate() &&
            month === new Date().getMonth() &&
            year === new Date().getFullYear();

        grid.innerHTML += `
        <div class="day ${isToday ? "active" : ""}" 
             onclick="selectDate(${i})">
            ${i}
        </div>`;
    }

    document.querySelector(".mini-header span").innerText =
        currentDate.toLocaleDateString("en-US", { month: "long", year: "numeric" });
}
function changeMonth(step) {
    currentDate.setMonth(currentDate.getMonth() + step);
    renderMiniCalendar();
}

function getDaysInMonth(year, month) {
    return new Date(year, month + 1, 0).getDate();
}

function selectDate(day) {

    currentDate.setDate(day);

    // Xoá active cũ
    document.querySelectorAll(".day").forEach(d => d.classList.remove("active"));

    // Active lại cái đang click
    event.target.classList.add("active");

    updateMainHeader();
}
function updateMainHeader() {

    const header = document.querySelector(".calendar-header h3");

    header.innerText =
        currentDate.toLocaleDateString("en-US", {
            weekday: "long",
            day: "2-digit",
            month: "short",
            year: "numeric"
        });
}