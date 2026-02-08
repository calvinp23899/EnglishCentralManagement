let modalStudentId = null;
let modalClassId = null;

function openAddModal(studentId, classId) {
    modalStudentId = studentId;
    modalClassId = classId;

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
            location.reload(); 
        },
        error: function () {
            showToast("Action failed", "error");
        }
    });
}