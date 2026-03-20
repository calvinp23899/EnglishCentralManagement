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
        loadSectionContentItems();
    }

    if (tab === "footer") {
        loadFooter();
    }

    if (tab === "navbar") {
        loadNav();
    }
}
function reloadSections() {
    $.get('/Admin/Setting/GetSections', function (data) {
        var select = $('#sectionContentType');
        var currentVal = select.val(); // giữ lại giá trị đang chọn nếu có

        select.empty();
        select.append('<option value="">-- Select Section --</option>');
        $.each(data, function (i, item) {
            select.append(`<option value="${item.id}">${item.title}</option>`);
        });

        // Restore lại giá trị cũ nếu vẫn còn tồn tại
        select.val(currentVal);
    });
}
function loadSectionContentItems(page = 1) {
    $.ajax({
        url: "/Admin/Setting/LoadSectionContentItems",
        type: "GET",
        data: {
            page: page
        },
        success: function (html) {
            $("#sectionItem").html(html);
        },
        error: function () {
            showToast("Failed to load content", "error");
        }
    });
}

function loadFooter(page = 1) {
    $.ajax({
        url: "/Admin/Setting/LoadFooter",
        type: "GET",
        data: {
            page: page
        },
        success: function (html) {
            $("#footerContent").html(html);
        },
        error: function () {
            showToast("Failed to load footer", "error");
        }
    });
}

function loadNav(page = 1) {
    $.ajax({
        url: "/Admin/Setting/LoadSectionItem",
        type: "GET",
        data: {
            page: page
        },
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
            reloadSections();
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

function confirmDeleteSection(id) {
    openConfirmModal(
        'Do you want to delete this section? This action includes deleting section items',
        function () {
            DeleteSection(id);
        },
        'Delete Section',
        'Delete'
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

function DeleteSection(id) {
    $.ajax({
        url: "/Admin/Setting/DeleteSection",
        type: "POST",
        data: {
            id: id
        },
        success: function () {
            showToast("Section Deleted Successfully", "success");
            closeHeaderBodyDrawer();
            loadNav();
        },
        error: function (xhr) {
            const msg = xhr.responseText || "Failed to delete section";
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
            console.log("response: ", response);
            $("#HeaderBodyId").val(response.headerBodySectionId);
            $("#navTitle").val(response.navTitle);
            $("#headerbodyTitle").val(response.title);
            $("#headerbodyDescription").val(response.description);
            $("#headerbodyLink").val(response.link);
            $("#headerbodyOrder").val(response.order);
            $("#HeaderBodyId").val(response.headerBodySectionId);
            $("#headerbodyImageUrl").val(response.imageUrl);
            $("#sectionStatus").val(response.isActive);

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
    formData.append("IsActive", $("select[name='sectionStatus']").val());


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
        $("#sectionFormStatus").show();
        $("#btnCreateSection").hide();
    }

    if (drawerSessionMode === "create") {
        $("#headerBodyDrawer input, #headerBodyDrawer select, #headerBodyDrawer textarea").prop("disabled", false);
        $("#headerbodyImageUrl").prop("disabled", true);
        $("#btnSaveSection").hide();
        $("#sectionFormStatus").hide();
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

//================= FOOTER JS ===============
function openFooterDrawer() {
    $("#footerDrawer").addClass("active");
}

function closeFooterDrawer() {
    $("#footerDrawer").removeClass("active");
}
function confirmSaveFooter(id) {
    openConfirmModal(
        'Do you want to save this footer?',
        function () {
            saveFooter(id);
        },
        'Save Footer',
        'Save'
    );
}
function confirmDeleteFooter(id) {
    openConfirmModal(
        'Do you want to delete this footer?',
        function () {
            DeleteFooter(id);
        },
        'Delete Footer',
        'Delete'
    );
}
function DeleteFooter(id) {
    $.ajax({
        url: "/Admin/Setting/DeleteFooter",
        type: "POST",
        data: {
            id: id
        },
        success: function () {
            showToast("Footer Deleted Successfully", "success");
            closeFooterDrawer();
            loadFooter();
        },
        error: function (xhr) {
            const msg = xhr.responseText || "Failed to delete footer";
            showToast(msg, "error");
        }
    });
}
function createFooterDrawer() {
    clearFooterForm();
    applyFooterMode("create");
    openFooterDrawer();
}

function updateFooterDrawer(id) {
    clearFooterForm();
    loadFooterDetailData(id);
    applyFooterMode("edit");
    setTimeout(function () {
        openFooterDrawer();
    }, 500);
}

function clearFooterForm() {
    $("#footerDrawer select").val("");
    $("#footerDrawer textarea").val("");
    $("#footerDrawer input").val("");
}

function applyFooterMode(drawerSessionMode) {

    if (drawerSessionMode === "view") {

    }

    if (drawerSessionMode === "edit") {
        $("#btnSaveFooter").show();
        $("#footerIconDisplay").show();
        $("#footerFormStatus").show();
        $("#footerIconDisplay").prop("disabled", true);
        $("#btnCreateFooter").hide();
    }

    if (drawerSessionMode === "create") {

        $("#btnSaveFooter").hide();
        $("#footerIconDisplay").hide();
        $("#footerFormStatus").hide();
        $("#btnCreateFooter").show();
    }
}

function createFooter() {
    const formData = getFooterData();
    $.ajax({
        url: "/Admin/Setting/CreateFooter",
        type: "POST",
        data: formData,
        processData: false, 
        contentType: false, 
        success: function () {
            showToast("Footer created successfully", "success");
            closeFooterDrawer();
            loadFooter();
        },
        error: function (xhr) {
            const msg = xhr.responseText || "Failed to create footer";
            showToast(msg, "error");
        }
    });
}
function saveFooter(id) {
    const formData = getFooterData();
    $.ajax({
        url: "/Admin/Setting/UpdateFooter",
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            showToast("Footer updated successfully", "success");
            closeFooterDrawer();
            loadFooter();
        },
        error: function (xhr) {
            const msg = xhr.responseText || "Failed to update footer";
            showToast(msg, "error");
        }
    });
}

function loadFooterDetailData(id) {
    $.ajax({
        url: "/Admin/Setting/GetFooterDetail",
        type: "GET",
        data: {
            id: id
        },
        success: function (response) {
            $("#FooterItemId").val(response.id);
            $("select[name='footerType']").val(response.footerEnum);
            $("#footerDescription").val(response.description);
            $("#footerLink").val(response.link);
            $("#footerOrder").val(response.order);
            $("#footerIconDisplay").val(response.icon);
            $("select[name='footerIcon']").val(response.iconEnum);
            $("select[name='footerStatus']").val(response.isActive);     
        },
        error: function (xhr) {
            const msg = xhr.responseText || "Failed to load footer";
            showToast(msg, "error");
        }
    });
}
function getFooterData() {
    const formData = new FormData();
    formData.append("Id", $("#FooterItemId").val());
    formData.append("FooterEnum", $("select[name='footerType']").val());
    formData.append("Description", $("#footerDescription").val());
    formData.append("Link", $("#footerLink").val());
    formData.append("Order", $("#footerOrder").val());
    formData.append("IconEnum", $("select[name='footerIcon']").val());
    formData.append("IsActive", $("select[name='footerStatus']").val());
    return formData;
}


//=================CONTENT JS ==============
function openContentDrawer() {
    $("#contentDrawer").addClass("active");
}

function closeContentDrawer() {
    $("#contentDrawer").removeClass("active");
}
function confirmSaveContentItem(id) {
    openConfirmModal(
        'Do you want to save this section item?',
        function () {
            saveContent(id);
        },
        'Save Section Item',
        'Save'
    );
}
function createContentDrawer() {
    clearContentForm();
    applyContentMode("create");
    openContentDrawer();
}

function updateContentDrawer(id) {
    clearContentForm();
    loadContentDetailData(id);
    applyContentMode("edit");
    setTimeout(function () {
        openContentDrawer();
    }, 500);
}

function confirmDeleteContent(id) {
    openConfirmModal(
        'Do you want to delete this content?',
        function () {
            DeleteContent(id);
        },
        'Delete Footer',
        'Delete'
    );
}
function DeleteContent(id) {
    $.ajax({
        url: "/Admin/Setting/DeleteSectionItem",
        type: "POST",
        data: {
            id: id
        },
        success: function () {
            showToast("Content Deleted Successfully", "success");
            closeContentDrawer();
            loadSectionContentItems();
        },
        error: function (xhr) {
            const msg = xhr.responseText || "Failed to delete content";
            showToast(msg, "error");
        }
    });
}

function clearContentForm() {
    $("#contentDrawer select").val("");
    $("#contentDrawer textarea").val("");
    $("#contentDrawer input").val("");
}

function applyContentMode(drawerSessionMode) {
    $("#contentImgUrl").prop("disabled", true);

    if (drawerSessionMode === "view") {

    }

    if (drawerSessionMode === "edit") {
        $("#btnSaveContent").show();
        $("#contentFormStatus").show();
        $("#btnCreateContent").hide();
    }

    if (drawerSessionMode === "create") {

        $("#btnSaveContent").hide();
        $("#contentFormStatus").hide();
        $("#btnCreateContent").show();
    }
}

function createContent() {
    const formData = getContentData();
    $.ajax({
        url: "/Admin/Setting/CreateSectionContent",
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            showToast("SectionContent created successfully", "success");
            closeContentDrawer();
            loadSectionContentItems();
        },
        error: function (xhr) {
            const msg = xhr.responseText || "Failed to create content";
            showToast(msg, "error");
        }
    });
}
function saveContent(id) {
    const formData = getContentData();
    $.ajax({
        url: "/Admin/Setting/UpdateSectionContent",
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            showToast("Content updated successfully", "success");
            closeContentDrawer();
            loadSectionContentItems();
        },
        error: function (xhr) {
            const msg = xhr.responseText || "Failed to update content";
            showToast(msg, "error");
        }
    });
}

function loadContentDetailData(id) {
    $.ajax({
        url: "/Admin/Setting/GetContentDetail",
        type: "GET",
        data: {
            id: id
        },
        success: function (response) {
            $("#ContentItemId").val(response.id);
            $("select[name='sectionContentType']").val(response.headerBodySectionId);
            $("select[name='contentStatus']").val(response.isActive);
            $("#contentTitle").val(response.title);
            $("#contentDescription").val(response.description);
            $("#contentOrder").val(response.order);
            $("#contentLink").val(response.link);
            $("#contentImgUrl").val(response.imageUrl);
        },
        error: function (xhr) {
            const msg = xhr.responseText || "Failed to load footer";
            showToast(msg, "error");
        }
    });
}
function getContentData() {
    const formData = new FormData();
    formData.append("Id", $("#ContentItemId").val());
    formData.append("HeaderBodySectionId", $("select[name='sectionContentType']").val());
    formData.append("IsActive", $("select[name='contentStatus']").val());
    formData.append("Title", $("#contentTitle").val());
    formData.append("Description", $("#contentDescription").val());
    formData.append("Order", $("#contentOrder").val());
    formData.append("Link", $("#contentLink").val());

    const imageFile = $("#contentImage")[0].files[0];
    if (imageFile) {
        formData.append("Image", imageFile);
    }

    return formData;
}
