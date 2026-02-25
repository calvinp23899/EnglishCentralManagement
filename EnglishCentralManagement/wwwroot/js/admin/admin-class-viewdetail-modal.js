// ================= TAB =================
document.querySelectorAll("[data-tab-scope]").forEach(scope => {

    const scopeName = scope.dataset.tabScope;
    const storageKey = "active-tab-" + scopeName;

    const buttons = scope.querySelectorAll(".tab-btn");
    const panes = scope.parentElement.querySelectorAll(".tab-pane");

    buttons.forEach(btn => {
        btn.addEventListener("click", () => {

            // clear active
            buttons.forEach(b => b.classList.remove("active"));
            panes.forEach(p => p.classList.remove("active"));

            // set active
            btn.classList.add("active");

            const tab = btn.dataset.tab;
            const pane = scope.parentElement.querySelector("#tab-" + tab);
            if (pane) pane.classList.add("active");

            // lưu tab
            sessionStorage.setItem(storageKey, tab);
        });
    });

    // ===== Restore khi F5 =====
    const savedTab = sessionStorage.getItem(storageKey);
    if (savedTab) {
        const btn = scope.querySelector(`.tab-btn[data-tab="${savedTab}"]`);
        if (btn) btn.click();
    }
});


// ================= MODAL =================
function openAddModal(studentId) {
    const input = document.getElementById("modalStudentId");
    const modal = document.getElementById("addStudentModal");

    if (!input || !modal) return;

    input.value = studentId;
    modal.style.display = "flex";
}

function closeModal() {
    const modal = document.getElementById("addStudentModal");
    if (modal) modal.style.display = "none";
}


