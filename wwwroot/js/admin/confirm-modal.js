let confirmCallback = null;

function openConfirmModal(message, onConfirm, title = 'Confirm') {
    document.getElementById('confirmMessage').innerText = message;
    document.getElementById('confirmTitle').innerText = title;

    confirmCallback = onConfirm;
    document.getElementById('confirmModal').classList.remove('hidden');
}

function closeConfirmModal() {
    document.getElementById('confirmModal').classList.add('hidden');
    confirmCallback = null;
}

document.getElementById('confirmCancel').onclick = closeConfirmModal;

document.getElementById('confirmOk').onclick = function () {
    if (confirmCallback) confirmCallback();
    closeConfirmModal();
};

function submitDeleteTeacher(id) {
    $.ajax({
        url: '/Admin/Teacher/Delete',
        type: 'POST',
        data: {
            id: id,
            __RequestVerificationToken: $('#antiForgeryForm input[name="__RequestVerificationToken"]').val()
        },
        success: function () {
            showToast('Delete Teacher Successfully');
            $('#teacher-row-' + id).remove();
        },
        error: function (xhr) {
            console.error(xhr.responseText);
            showToast('Action Failed', 'error');
        }
    });
}

function submitDeleteStudent(id) {
    $.ajax({
        url: '/Admin/Student/Delete',
        type: 'POST',
        data: {
            id: id,
            __RequestVerificationToken: $('#antiForgeryForm input[name="__RequestVerificationToken"]').val()
        },
        success: function () {
            showToast('Delete Student Successfully');
            $('#student-row-' + id).remove();
        },
        error: function (xhr) {
            console.error(xhr.responseText);
            showToast('Action Failed', 'error');
        }
    });
}

function submitDeleteClass(id) {
    $.ajax({
        url: '/Admin/Class/Delete',
        type: 'POST',
        data: {
            id: id,
            __RequestVerificationToken: $('#antiForgeryForm input[name="__RequestVerificationToken"]').val()
        },
        success: function () {
            showToast('Delete Class Successfully');
            $('#class-row-' + id).remove();
        },
        error: function (xhr) {
            console.error(xhr.responseText);
            showToast('Action Failed', 'error');
        }
    });
}
//=========== Hàm Run
function confirmDeleteTeacher(id) {
    openConfirmModal(
        'Do you want to delete this teacher?',
        function () {
            submitDeleteTeacher(id);
        },
        'Delete Teacher'
    );
}

function confirmDeleteStudent(id) {
    openConfirmModal(
        'Do you want to delete this student?',
        function () {
            submitDeleteStudent(id);
        },
        'Delete Student'
    );
}

function confirmDeleteClass(id) {
    openConfirmModal(
        'Do you want to delete this class?',
        function () {
            submitDeleteClass(id);
        },
        'Delete Class'
    );
}


