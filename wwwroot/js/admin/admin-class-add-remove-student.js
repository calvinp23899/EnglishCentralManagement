let modalStudentId = null;
let modalClassId = null;

function openAddModal(studentId, classId) {
    modalStudentId = studentId;
    modalClassId = classId;

    document.getElementById("modalText").innerText =
        "Do you want to add this student to class?";

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
            location.reload(); 
        },
        error: function () {
            showToast("Action failed", "error");
        }
    });
}