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
    showToast('Xoá teacher thành công');
    console.log("id: ", id);
    $.ajax({
        url: '/Admin/Teacher/Delete',
        type: 'POST',
        data: {
            id: id,
            __RequestVerificationToken: $('#antiForgeryForm input[name="__RequestVerificationToken"]').val()
        },
        success: function () {
            showToast('Xoá teacher thành công');
            $('#teacher-row-' + id).remove();
        },
        error: function (xhr) {
            console.error(xhr.responseText);
            showToast('Xoá teacher thất bại', 'error');
        }
    });
}

function submitDeleteStudent(id) {
    showToast('Xoá student thành công');
    console.log("id: ", id);
    $.ajax({
        url: '/Admin/Student/Delete',
        type: 'POST',
        data: {
            id: id,
            __RequestVerificationToken: $('#antiForgeryForm input[name="__RequestVerificationToken"]').val()
        },
        success: function () {
            showToast('Xoá student thành công');
            $('#student-row-' + id).remove();
        },
        error: function (xhr) {
            console.error(xhr.responseText);
            showToast('Xoá student thất bại', 'error');
        }
    });
}
//=========== Hàm Run
function confirmDeleteTeacher(id) {
    openConfirmModal(
        'Bạn có chắc muốn xoá teacher này?',
        function () {
            submitDeleteTeacher(id);
        },
        'Delete Teacher'
    );
}

function confirmDeleteStudent(id) {
    openConfirmModal(
        'Bạn có chắc muốn xoá student này?',
        function () {
            submitDeleteStudent(id);
        },
        'Delete Student'
    );
}


