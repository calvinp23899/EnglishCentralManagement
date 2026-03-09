function showToast(message, type = 'success', duration = 3000) {
    const toast = document.getElementById('toast');
    const msg = document.getElementById('toastMessage');

    msg.innerText = message;

    toast.classList.remove('hidden', 'toast--error', 'toast--success', 'toast--warning', 'toast--progress');

    if (type === 'success') {
        toast.classList.add('toast--success');
    }

    if (type === 'error') {
        toast.classList.add('toast--error');
    }

    if (type === 'warning') {
        toast.classList.add('toast--warning');
    }

    if (type === 'progress') {
        toast.classList.add('toast--progress');
    }

    setTimeout(() => {
        toast.classList.add('hidden');
    }, duration);
}
