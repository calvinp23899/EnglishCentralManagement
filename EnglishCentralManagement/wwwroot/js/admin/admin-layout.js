document.addEventListener("DOMContentLoaded", function () {

    const sidebar = document.querySelector(".sidebar");
    const toggleBtn = document.querySelector(".toggle-btn");
    const userDropdown = document.querySelector(".user-dropdown");
    const dropdownMenu = document.querySelector(".dropdown-menu");

    /* ===== SIDEBAR COLLAPSE ===== */
    if (toggleBtn && sidebar) {

        // load state
        if (localStorage.getItem("sidebar-collapsed") === "true") {
            sidebar.classList.add("collapsed");
        }

        toggleBtn.addEventListener("click", () => {
            sidebar.classList.toggle("collapsed");
            localStorage.setItem(
                "sidebar-collapsed",
                sidebar.classList.contains("collapsed")
            );
        });
    }

    /* ===== SUB MENU (USER) ===== */
    document.querySelectorAll(".menu-item.has-sub .menu-link")
        .forEach(link => {
            link.addEventListener("click", () => {

                if (!sidebar) return;

                // nếu sidebar đang collapse → mở ra trước
                if (sidebar.classList.contains("collapsed")) {
                    sidebar.classList.remove("collapsed");
                    localStorage.setItem("sidebar-collapsed", "false");
                }

                link.parentElement.classList.toggle("open");
            });
        });

    /* ===== USER DROPDOWN ===== */
    if (userDropdown && dropdownMenu) {
        userDropdown.addEventListener("click", (e) => {
            e.stopPropagation();
            dropdownMenu.classList.toggle("show");
        });

        document.addEventListener("click", () => {
            dropdownMenu.classList.remove("show");
        });
    }

});

