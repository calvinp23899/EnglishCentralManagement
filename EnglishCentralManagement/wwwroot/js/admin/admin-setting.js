$(document).ready(function () {
    const settingScope = document.querySelector('[data-tab-scope="setting"]');

    if (!settingScope) return;
    let activeSettingTab = settingScope.querySelector(".tab-btn.active")
        ?? settingScope.querySelector(".tab-btn");
    if (!activeSettingTab) return;

    activeSettingTab.classList.add("active");
    const tab = activeSettingTab.dataset.tab;
    loadSettingTab(tab);
    // Khi click => load data tab
    settingScope.querySelectorAll(".tab-btn").forEach(btn => {
        btn.addEventListener("click", function () {
            loadSettingTab(btn.dataset.tab);
        });
    });
})

function loadSettingTab(tab) {
    if (tab === "body") {
        loadSlider();
    }

    if (tab === "footer") {
        loadCourse();
    }

    if (tab === "navbar") {
        loadNav();
    }
}

function loadSlider() {
    $.ajax({
        url: "/Admin/Setting/LoadSlider",
        type: "GET",
        success: function (html) {
            $("#sliderContent").html(html);
        },
        error: function () {
            showToast("Failed to load slider", "error");
        }
    });
}

function loadCourse() {
    $.ajax({
        url: "/Admin/Setting/LoadCourse",
        type: "GET",
        success: function (html) {
            $("#courseContent").html(html);
        },
        error: function () {
            showToast("Failed to load slider", "error");
        }
    });
}

function loadNav() {
    console.log("load nav");
    $.ajax({
        url: "/Admin/Setting/LoadSectionItem",
        type: "GET",
        success: function (html) {
            $("#sectionContent").html(html);
        },
        error: function () {
            showToast("Failed to load nav items", "error");
        }
    });
}

function openHeaderBodyDrawer() {
    $("#headerBodyDrawer").addClass("active");
}

function closeHeaderBodyDrawer() {
    $("#headerBodyDrawer").removeClass("active");
}

function initSectionForm() {
    $('input[name="sectionType"][value="isNav"]').prop("checked", true).trigger("change");
}

function createSection() {
    const formDataSection = getDataSection();
    $.ajax({
        url: "/Admin/Setting/CreateSection",
        type: "POST",
        data: formDataSection,
        processData: false,   
        contentType: false, 
        success: function () {
            showToast("Section created successfully", "success");
            closeHeaderBodyDrawer();
            loadNav();
        },
        error: function (xhr) {
            const msg = xhr.responseText || "Failed to create section";
            showToast(msg, "error");
        }
    });
}
function confirmSaveSection(id) {
    openConfirmModal(
        'Do you want to save this section?',
        function () {
            saveSection(id);
        },
        'Save Section',
        'Save'
    );
}
function saveSection(id) {
    const formDataSection = getDataSection();
    $.ajax({
        url: "/Admin/Setting/UpdateSection",
        type: "POST",
        data: formDataSection,
        processData: false,
        contentType: false,
        success: function () {
            showToast("Section updated successfully", "success");
            closeHeaderBodyDrawer();
            loadNav();
        },
        error: function (xhr) {
            const msg = xhr.responseText || "Failed to update section";
            showToast(msg, "error");
        }
    });
}

function loadSectionData(id) {
    $.ajax({
        url: "/Admin/Setting/GetSectionDetail",
        type: "GET",
        data: {
            id: id
        },
        success: function (response) {
            $("#HeaderBodyId").val(response.headerBodySectionId);
            $("#navTitle").val(response.navTitle);
            $("#headerbodyTitle").val(response.title);
            $("#headerbodyDescription").val(response.description);
            $("#headerbodyLink").val(response.link);
            $("#headerbodyOrder").val(response.order);
            $("#HeaderBodyId").val(response.headerBodySectionId);
            $("#headerbodyImageUrl").val(response.imageUrl);

            // set radio button
            if (response.isNav) {
                $('input[name="sectionType"][value="isNav"]').prop("checked", true);
                $('input[name="sectionType"][value="isSlider"]').prop("checked", false);
                $('input[name="sectionType"][value="isNav"]').trigger("change");
                $("#headerbodyImage").prop("disabled", true);
            } else {
                $('input[name="sectionType"][value="isNav"]').prop("checked", false);
                $('input[name="sectionType"][value="isSlider"]').prop("checked", true);
                $('input[name="sectionType"][value="isSlider"]').trigger("change");
                $("#headerbodyImage").prop("disabled", false);
            }
        },
        error: function (xhr) {
            const msg = xhr.responseText || "Failed to update section";
            showToast(msg, "error");
        }
    });
}
function getDataSection() {
    const formData = new FormData();
    const isNav = $('input[name="sectionType"]:checked').val() === "isNav";
    formData.append("IsNav", isNav);
    formData.append("IsSlider", !isNav);
    formData.append("HeaderBodySectionId", $("#HeaderBodyId").val() || 0);
    formData.append("NavTitle", $("#navTitle").val());
    formData.append("Title", $("#headerbodyTitle").val());
    formData.append("Description", $("#headerbodyDescription").val());
    formData.append("Link", $("#headerbodyLink").val());
    formData.append("Order", $("#headerbodyOrder").val());

    const imageFile = $("#headerbodyImage")[0].files[0];
    if (imageFile) {
        formData.append("Image", imageFile);
    }
    return formData;
}

function createSectionDrawer() {
    clearForm();
    applySectionMode("create");
    openHeaderBodyDrawer();
    initSectionForm();
}

function updateSectionDrawer(id) {
    clearForm();
    loadSectionData(id);
    applySectionMode("edit");
    setTimeout(function () {
        openHeaderBodyDrawer();
    }, 500);
}

function clearForm() {
    $("#headerBodyDrawer input:not([type='radio'])").val("");
    $("#headerBodyDrawer select").val("");
    $("#headerBodyDrawer textarea").val("");
}

function applySectionMode(drawerSessionMode) {

    if (drawerSessionMode === "view") {
        $("#headerBodyDrawer input, #headerBodyDrawer select, #headerBodyDrawer textarea").prop("disabled", true);
        $("#btnSaveSection").hide();
        $("#btnCreateSection").hide();
    }

    if (drawerSessionMode === "edit") {
        $("#headerBodyDrawer input, #headerBodyDrawer select, #headerBodyDrawer textarea").prop("disabled", false);
        $('input[name="sectionType"]').prop("disabled", true);  // ← disable radio
        $("#headerbodyImageUrl").prop("disabled", true);
        $("#btnSaveSection").show();
        $("#btnCreateSection").hide();
    }

    if (drawerSessionMode === "create") {
        $("#headerBodyDrawer input, #headerBodyDrawer select, #headerBodyDrawer textarea").prop("disabled", false);
        $("#headerbodyImageUrl").prop("disabled", true);
        $("#btnSaveSection").hide();
        $("#btnCreateSection").show();
    }
}
$(document).on("change", 'input[name="sectionType"]', function () {
    const isNav = $(this).val() === "isNav";
    if (isNav) {
        $("#headerbodyImage").prop("disabled", true).val("");
        $("#navTitle").prop("disabled", false);
        $("#headerbodyTitle").prop("disabled", false);
        $("#headerbodyDescription").prop("disabled", false);
        $("#headerbodyLink").prop("disabled", false);
    } else {
        $("#headerbodyImage").prop("disabled", false);
        $("#navTitle").prop("disabled", true);
        $("#headerbodyTitle").prop("disabled", true);
        $("#headerbodyDescription").prop("disabled", true);
        $("#headerbodyImageUrl").prop("disabled", true);
    }
});