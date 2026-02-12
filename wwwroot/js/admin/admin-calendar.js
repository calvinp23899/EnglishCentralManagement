var currentDate = selectedDateFromServer
    ? new Date(selectedDateFromServer)
    : new Date();
var currentView = new URLSearchParams(window.location.search).get("view") || "day";

document.addEventListener("DOMContentLoaded", function () {

    const panel = document.querySelector(".event-detail-panel");

    // ===== EVENT DELEGATION =====
    document.addEventListener("click", function (e) {

        const eventEl = e.target.closest(".event");
        if (!eventEl) return;

        if (!panel) return;

        panel.classList.add("active");

        // Lấy data từ dataset
        document.getElementById("eventName").value =
            eventEl.dataset.name || "";

        document.getElementById("linkMeeting").value =
            eventEl.dataset.link || "";

        document.getElementById("startDate").value =
            eventEl.dataset.startdate || "";

        document.getElementById("endDate").value =
            eventEl.dataset.enddate || "";

        document.getElementById("startTime").value =
            eventEl.dataset.starttime || "";

        document.getElementById("endTime").value =
            eventEl.dataset.endtime || "";

        document.getElementById("eventId").value =
            eventEl.dataset.id || "";

        // ===== CHECKBOX REPEAT =====
        const days = ["mon", "tue", "wed", "thu", "fri", "sat", "sun"];

        days.forEach(day => {
            const checkbox = document.getElementById("is" + day.charAt(0).toUpperCase() + day.slice(1));
            if (checkbox) {
                checkbox.checked =
                    eventEl.dataset["is" + day]?.toLowerCase() === "true";
            }
        });


        // Lưu id vào panel để Save/Delete dùng
        panel.dataset.eventId = eventEl.dataset.id;
    });

    const closeBtn = document.querySelector(".close-btn");
    if (closeBtn) {
        closeBtn.addEventListener("click", function () {
            if (panel) panel.classList.remove("active");
        });
    }

    if (document.getElementById("miniGrid")) {
        renderMiniCalendar();
    }

    const radios = document.querySelectorAll("input[name='calendarView']");

    radios.forEach(r => {
        r.addEventListener("change", function () {

            const selectedView = this.value;

            if (selectedView === "day") {
                window.location.href = "?view=day";
            }
            else if (selectedView === "week") {
                window.location.href = "?view=week";
            }
        });
    });

});

function openEventCalendarModal() {
    $("#eventCalendarModal").addClass("active");
}

function closeEventModal() {
    $("#eventCalendarModal").removeClass("active");
}


function renderMiniCalendar() {
    const year = currentDate.getFullYear();
    const month = currentDate.getMonth();

    const grid = document.getElementById("miniGrid");
    grid.innerHTML = ""; // clear trước

    const daysInMonth = getDaysInMonth(year, month);

    // Day names
    const dayNames = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
    dayNames.forEach(d => {
        grid.innerHTML += `<div class="day-name">${d}</div>`;
    });

    // First day of month
    const firstDay = new Date(year, month, 1).getDay();

    // Empty slots
    for (let i = 0; i < firstDay; i++) {
        grid.innerHTML += `<div></div>`;
    }

    // Days
    for (let i = 1; i <= daysInMonth; i++) {

        const isSelected =
            selectedDateFromServer &&
            i === new Date(selectedDateFromServer).getDate() &&
            month === new Date(selectedDateFromServer).getMonth() &&
            year === new Date(selectedDateFromServer).getFullYear();

        grid.innerHTML += `
    <div class="day ${isSelected ? "active" : ""}" 
         onclick="selectDate(${year}, ${month}, ${i})">
        ${i}
    </div>`;
    }

    document.getElementById("miniMonthYear").innerText =
        currentDate.toLocaleDateString("en-US", {
            month: "long",
            year: "numeric"
        });

}
function changeMonth(step) {
    currentDate.setMonth(currentDate.getMonth() + step);
    renderMiniCalendar();
    /*updateMainHeader();*/
}

function getDaysInMonth(year, month) {
    return new Date(year, month + 1, 0).getDate();
}

function selectDate(year, month, day) {
    const selected = new Date(year, month, day);

    const formatted =
        selected.getFullYear() + "-" +
        String(selected.getMonth() + 1).padStart(2, '0') + "-" +
        String(selected.getDate()).padStart(2, '0');

    window.location.href = "/Admin/Calendar/Index?date=" + formatted;
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

function saveEvent() {
    const url = "/Admin/Calendar/CreateEvent";

    $.ajax({
        url: url,
        type: "POST",
        data: {
            EventName: $("#EventName").val(),
            LinkMeeting: $("#LinkMeeting").val(),

            StartDate: $("#StartDate").val(),
            EndDate: $("#EndDate").val(),

            StartTime: $("#StartTime").val(),
            EndTime: $("#EndTime").val(),

            IsMonday: $("#IsMonday").is(":checked"),
            IsTuesday: $("#IsTuesday").is(":checked"),
            IsWednesday: $("#IsWednesday").is(":checked"),
            IsThursday: $("#IsThursday").is(":checked"),
            IsFriday: $("#IsFriday").is(":checked"),
            IsSaturday: $("#IsSaturday").is(":checked"),
            IsSunday: $("#IsSunday").is(":checked"),
            __RequestVerificationToken:
                $('#antiForgeryForm input[name="__RequestVerificationToken"]').val()
        },
        success: function (response) {
            showToast("Add event success");
            closeEventModal();
            setTimeout(function () {
                window.location.href = "/Admin/Calendar/Index?date=" +
                    $("#StartDate").val() +
                    "&view=" + currentView;
            }, 1500); // delay 1.5s

        },
        error: function (err) {
            showToast("Action failed", "error");
        }
    });
}

function EditEvent() {
    const url = "/Admin/Calendar/EditEvent";

    $.ajax({
        url: url,
        type: "POST",
        data: {
            EventId: $("#eventId").val(),
            EventName: $("#eventName").val(),
            LinkMeeting: $("#linkMeeting").val(),

            StartDate: $("#startDate").val(),
            EndDate: $("#endDate").val(),

            StartTime: $("#startTime").val(),
            EndTime: $("#endTime").val(),

            IsMonday: $("#isMon").is(":checked"),
            IsTuesday: $("#isTue").is(":checked"),
            IsWednesday: $("#isWed").is(":checked"),
            IsThursday: $("#isThu").is(":checked"),
            IsFriday: $("#isFri").is(":checked"),
            IsSaturday: $("#isSat").is(":checked"),
            IsSunday: $("#isSun").is(":checked"),
            __RequestVerificationToken:
                $('#antiForgeryForm input[name="__RequestVerificationToken"]').val()
        },
        success: function (response) {
            showToast("Update event success");
            closeEventModal();
            setTimeout(function () {
                window.location.href = "/Admin/Calendar/Index?date=" +
                    $("#StartDate").val() +
                    "&view=" + currentView;
            }, 1500); // delay 1.5s

        },
        error: function (err) {
            showToast("Action failed", "error");
        }
    });
}

function confirmDeleteEvent() {
    openConfirmModal(
        'Do you want to delete this event?',
        function () {
            DeleteEvent();
        },
        'Delete Event'
    );
}

function confirmEditEvent() {
    openConfirmModal(
        'Do you want to update this event?',
        function () {
            EditEvent();
        },
        'Update Event',
        'Save changes'
    );
}

function DeleteEvent() {
    const url = "/Admin/Calendar/DeleteEvent";

    $.ajax({
        url: url,
        type: "POST",
        data: {
            EventId: $("#eventId").val(),          
            __RequestVerificationToken:
                $('#antiForgeryForm input[name="__RequestVerificationToken"]').val()
        },
        success: function (response) {
            showToast("Delete event success");
            closeEventModal();
            setTimeout(function () {
                window.location.href = "/Admin/Calendar/Index?date=" +
                    $("#StartDate").val() +
                    "&view=" + currentView;
            }, 1500); // delay 1.5s

        },
        error: function (err) {
            showToast("Action failed", "error");
        }
    });
}