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
    //document.getElementById('deleteTeacherId').value = id;
    //document.getElementById('deleteTeacherForm').submit();
    // giả lập logic delete
    console.log('Deleting teacher id:', id);

    showToast('Xoá teacher thành công');
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


