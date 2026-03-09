let modalStudentId = null;
let modalClassId = null;
$(document).ready(function () {
    const classDetailScope = document.querySelector('[data-tab-scope="class-detail"]');

    if (!classDetailScope) return; // nếu không phải trang classdetail thì thoát

    const activeTabClassDetail = classDetailScope.querySelector(".tab-btn.active");

    if (!activeTabClassDetail) return;
    const tabClassDetail = activeTabClassDetail.dataset.tab;
    const classId = $("#classIdHidden").val();
    modalClassId = classId;
    loadClassDetailTab(tabClassDetail);
    //// Khi click => load data tab
    classDetailScope.querySelectorAll(".tab-btn").forEach(btn => {
        btn.addEventListener("click", function () {
            loadClassDetailTab(btn.dataset.tab);
        });
    });


    //GetStudentAddTable(1);
});

function loadClassDetailTab(tab) {

    if (tab === "students") {
        GetStudentAddTable(1);
    }
}
function openAddModal(studentId) {
    modalStudentId = studentId;
    modalClassId = $("#classIdHidden").val();
    console.log("ClassIdHidden: ", modalClassId);

    document.getElementById("modalText").innerHTML = `
    <strong>Do you want to add this student to class?</strong><br>
    <small class="text-muted">
        This action includes adding student to class and creating contract
    </small>
`;

    document.getElementById("StudentModal").style.display = "flex";
}

function closeModal() {
    document.getElementById("StudentModal").style.display = "none";
}
function confirmAction() {
    const url = "/Admin/Class/AddStudentToClass";

    $.ajax({
        url: url,
        type: "POST",
        data: {
            studentId: modalStudentId,
            classId: modalClassId,
            __RequestVerificationToken:
                $('#antiForgeryForm input[name="__RequestVerificationToken"]').val()
        },
        success: function () {
            showToast("Add student success");
            closeModal();
            GetStudentAddTable();
            location.reload(); 
        },
        error: function (xhr) {

            var message = xhr.responseText;

            showToast(message, "error");
        }
    });
}

function GetStudentAddTable(page = 1) {
    const url = "/Admin/Class/SearchStudentToAdd";
    const search = $("#searchStudentAdd").val();
    $.ajax({
        url: url,
        type: "GET",
        data: {
            classId: modalClassId,
            search: search,
            page: page,
            __RequestVerificationToken:
                $('#antiForgeryForm input[name="__RequestVerificationToken"]').val()
        },
        success: function (html) {
            $("#StudentAddTableContent").html(html);
        },
        error: function (xhr) {

            var message = xhr.responseText;

            showToast(message, "error");
        }
    });
}