//document.querySelectorAll("[data-tab-scope]").forEach(btn => {
//    btn.addEventListener("click", () => {
//        document.querySelectorAll(".tab-btn").forEach(b => b.classList.remove("active"));
//        document.querySelectorAll(".tab-pane").forEach(p => p.classList.remove("active"));

//        btn.classList.add("active");
//        document.getElementById("tab-" + btn.dataset.tab).classList.add("active");

//        //Giữ State tab khi f5
//        localStorage.setItem(storageKey, btn.dataset.tab);
//    });
//});

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
            localStorage.setItem(storageKey, tab);
        });
    });

    // ===== Restore khi F5 =====
    const savedTab = localStorage.getItem(storageKey);
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

////========== Modal ===========
//function openAddModal(studentId) {
//    document.getElementById("modalStudentId").value = studentId;
//    document.getElementById("addStudentModal").style.display = "flex";
//}

//function closeModal() {
//    document.getElementById("addStudentModal").style.display = "none";
//}

////========== Giữ State Tab Khi F5 ===========
//const storageKey = "class-detail-active-tab";
//const activeTab = localStorage.getItem("classTab");

//if (activeTab) {
//    const btn = document.querySelector(`.tab-btn[data-tab="${activeTab}"]`);
//    if (btn) {
//        btn.click(); // giả lập click để active lại tab
//    }
//}
