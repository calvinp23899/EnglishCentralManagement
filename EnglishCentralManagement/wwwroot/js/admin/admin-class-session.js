$(document).ready(function () {
    const myClassScope = document.querySelector('[data-tab-scope="myclass"]');

    if (!myClassScope) return; 

    const activeClassTab = myClassScope.querySelector(".tab-btn.active");

    if (!activeClassTab) return;
    const tabClass = activeClassTab.dataset.tab;
    loadMyClassTab(tabClass);
    // Khi click tab => load data 
    myClassScope.querySelectorAll(".tab-btn").forEach(btn => {
        btn.addEventListener("click", function () {
            loadMyClassTab(btn.dataset.tab);
        });
    });
});

function loadMyClassTab(tab) {

    if (tab === "sessioninfo") {
        loadSession(1);
    }
}

function loadSession(page = 1, searchSessionName = null) {
    const myClassId = $('#myClassId').val();
    $.ajax({
        url: "/Admin/Class/LoadSessionTab",
        type: "GET",
        data: {
            classId: myClassId,
            searchSessionName: searchSessionName,
            page: page
        },
        success: function (html) {
            $("#classSessionContent").html(html);
        },
        error: function () {
            showToast("Failed to load session data", "error");
        }
    });
}

function searchSessionName() {
    const searchSession = $('#searchSessionName').val();
    loadSession(1, searchSession);
}

function loadAttendanceStudents(sessionId) {
    $("#sessionIdHidden").val(sessionId);
    $.ajax({
        url: "/Admin/Class/GetStudentsBySession",
        type: "GET",
        data: {
            id: sessionId,
            classId: $('#myClassId').val()
        },
        success: function (students) {

            let html = "";

            students.forEach(s => {
                const presentChecked = s.status === 1 ? "checked" : "";
                const absentChecked = s.status === 2 ? "checked" : "";
                html += `
                    <tr data-student-id="${s.studentId}">
                        <td>${s.fullName}</td>
                        <td>
                            <input type="radio"
                                   name="attendance_${s.studentId}" 
                                   value="1" 
                                   ${presentChecked} />
                        </td>
                        <td>
                            <input type="radio" 
                                   name="attendance_${s.studentId}" 
                                   value="2" 
                                   ${absentChecked} />
                        </td>
                    </tr>
                `;
            });

            $("#attendanceList").html(html);
            viewAttendanceDrawer();
        },
        error: function (xhr) {
            const res = JSON.parse(xhr.responseText);
            showToast(res.message, "error");
        }
    });
}

function confirmSaveAttendance() {
    openConfirmModal(
        'Do you want to save this session attendance?',
        function () {
            saveAttendance();
        },
        'Save Attendance',
        "Save"
    );
}


function saveAttendance() {
    var sessionIdHidden = $("#sessionIdHidden").val();
    let attendanceData = [];

    $("#attendanceList tr").each(function () {

        let studentId = $(this).data("student-id");

        let status = $(this)
            .find("input[type=radio]:checked")
            .val();

        attendanceData.push({
            StudentId: parseInt(studentId),
            Status: parseInt(status)
        });
    });

    $.ajax({
        url: "/Admin/Class/SaveAttendanceSession",
        type: "POST",
        data: {
            SessionId: sessionIdHidden,
            Students: attendanceData
        },
        success: function () {
            showToast("Saved!");
            closeAttendanceDrawer();
            loadSession(1);
        },
        error: function () {
            showToast("Action Failed!", "error");
        }
    });
}

function confirmSaveSession() {
    openConfirmModal(
        'Do you want to save this session?',
        function () {
            saveSession();
        },
        'Save Session',
        "Save"
    );
}

function confirmDeleteSession(id) {
    openConfirmModal(
        'Do you want to delete this session?',
        function () {
            deleteSession(id);
        },
        'Delete Session'
    );
}

function deleteSession(id) {
    $.ajax({
        url: "/Admin/Class/DeleteSession",
        type: "POST",
        data: {
            id: id
        },
        success: function (res) {
            showToast("Delete Session Successful");
            loadSession(1);
        },
        error: function () {
            showToast("Action Failed", "error");
        }
    });
}
function saveSession() {
    const model = {
        SessionName: $("#SessionName").val(),
        SessionDate: $("#SessionDate").val(),
        StartTime: $("#SessionStartTime").val(),
        EndTime: $("#SessionEndTime").val(),
        Status: $("#SessionStatus").val(),
        Note: $("#SessionNote").val(),
        FeedBack: $("#SessionFeedback").val(),
        ClassId: $('#myClassId').val(),
        Id: $('#SessionId').val()
    };

    $.ajax({
        url: "/Admin/Class/UpdateSession",
        type: "POST",
        data: model,
        success: function (res) {
            showToast("Update Session Successful");
            closeSessionDrawer();
            loadSession(1);
        },
        error: function () {
            showToast("Action Failed", "error");
        }
    });
}

function createSession() {
    const model = {
        SessionName: $("#SessionName").val(),
        SessionDate: $("#SessionDate").val(),
        StartTime: $("#SessionStartTime").val(),
        EndTime: $("#SessionEndTime").val(),
        Status: $("#SessionStatus").val(),
        Note: $("#SessionNote").val(),
        FeedBack: $("#SessionFeedback").val(),
        ClassId: $('#myClassId').val()
    };

    $.ajax({
        url: "/Admin/Class/CreateSession",
        type: "POST",
        data: model,
        success: function (res) {
            showToast("Create Session Successful");
            closeSessionDrawer();
            loadSession(1);
        },
        error: function (xhr) {
            const res = JSON.parse(xhr.responseText);
            showToast(res.message || "Action Failed", "error");
        }
    });
}
function loadSessionData(id) {
    $.ajax({
        url: "/Admin/Class/GetSessionDetail",
        type: "GET",
        data: {
            id: id,
        },
        success: function (data) {
            if (!data) {
                showToast("Session not found", "error");
                return;
            }

            // Hidden ID
            $("#SessionId").val(data.id);

            // Basic fields
            $("#SessionName").val(data.sessionName);
            $("#SessionNote").val(data.note);
            $("#SessionFeedback").val(data.feedBack);
            $("#SessionStatus").val(data.sessionStatusEnum);
            $("#SessionDate").val(data.sessionDate.split("T")[0]);
            $("#SessionStartTime").val(data.startTime.substring(0, 5));
            $("#SessionEndTime").val(data.endTime.substring(0, 5));

            // System fields
            $("#SessionCreatedBy").val(data.createdBy);
            $("#SessionCreatedDate").val(data.createdDate.split("T")[0]);
            $("#SessionUpdatedBy").val(data.updatedBy);
            $("#SessionUpdatedDate").val(data.updatedDate.split("T")[0]);
        },
        error: function (xhr) {
            const res = JSON.parse(xhr.responseText);
            showToast(res.message || "Action Load Session Data Failed", "error");
        }
    });
}

function viewAttendanceDrawer() {
    $("#attendanceDrawer").addClass("active");
}

function closeAttendanceDrawer() {
    $("#attendanceDrawer").removeClass("active");
}

function createSessionDrawer() {
    clearForm();
    applySessionMode("create");
    openSessionDrawer();
}

function editSessionDrawer(id) {
    clearForm();
    loadSessionData(id);
    applySessionMode("edit");
    openSessionDrawer();
}

function viewSessionDrawer(id) {
    clearForm();
    loadSessionData(id);
    applySessionMode("view");
    openSessionDrawer();
}

function clearForm() {
    $("#sessionDrawer input").val("");
    $("#sessionDrawer select").val("");
    $("#sessionDrawer textarea").val("");
}

function openSessionDrawer() {
    $("#sessionDrawer").addClass("active");
}

function closeSessionDrawer() {
    $("#sessionDrawer").removeClass("active");
}

function applySessionMode(drawerSessionMode) {

    if (drawerSessionMode === "view") {

        $("#sessionDrawer input, #sessionDrawer select, #sessionDrawer textarea").prop("disabled", true);

        $("#btnSaveSession").hide();
        $("#btnCreateSession").hide();
    }

    if (drawerSessionMode === "edit") {

        $("#sessionDrawer input, #sessionDrawer select, #sessionDrawer textarea").prop("disabled", false);

        // disable system fields
        $("#SessionCreatedBy, #SessionCreatedDate, #SessionUpdatedBy, #SessionUpdatedDate")
            .prop("disabled", true);

        $("#btnSaveSession").show();
        $("#btnCreateSession").hide();
    }

    if (drawerSessionMode === "create") {

        $("#sessionDrawer input, #sessionDrawer select, #sessionDrawer textarea").prop("disabled", false);

        // system fields không cho nhập khi create
        $("#SessionCreatedBy, #SessionCreatedDate, #SessionUpdatedBy, #SessionUpdatedDate")
            .prop("disabled", true);

        $("#btnSaveSession").hide();
        $("#btnCreateSession").show();
    }
}