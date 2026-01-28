function showToast(message, type = 'success', duration = 3000) {
    const toast = document.getElementById('toast');
    const msg = document.getElementById('toastMessage');

    msg.innerText = message;

    toast.classList.remove('hidden', 'toast--error');

    if (type === 'error') {
        toast.classList.add('toast--error');
    }

    setTimeout(() => {
        toast.classList.add('hidden');
    }, duration);
}
