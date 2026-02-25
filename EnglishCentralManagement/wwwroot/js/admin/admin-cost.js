document.addEventListener("DOMContentLoaded", function () {

    const costScope = document.querySelector('[data-tab-scope="cost"]');

    if (!costScope) return; // nếu không phải trang cost thì thoát

    const activeTab = costScope.querySelector(".tab-btn.active");

    if (!activeTab) return;
    const tab = activeTab.dataset.tab;
    loadCostTab(tab);
    // Khi click => load data tab
    costScope.querySelectorAll(".tab-btn").forEach(btn => {
        btn.addEventListener("click", function () {
            loadCostTab(btn.dataset.tab);
        });
    });
    

});


function loadCostTab(tab) {

    if (tab === "expense") {
        loadExpense(1);
    }

    if (tab === "profit") {
        loadProfit();
    }
}

function editExpense(id) {

    $.ajax({
        url: '/Admin/Cost/GetExpenseById',
        type: 'GET',
        data: { id: id },
        success: function (data) {

            $("#EditExpenseId").val(data.id);
            $("#EditExpenseCategory").val(data.category);
            $("#EditAmount").val(data.amount);
            $("#EditExpenseType").val(data.type);
            $("#EditExpenseDate").val(data.expenseDate);

            $("#EditNote").val(data.note);

            openExpenseDrawer();
        },
        error: function () {
            showToast("Action GET Edit Expense Failed", "error");
        }
    });
}

function saveExpense() {

    $.ajax({
        url: '/Admin/Cost/UpdateExpense',
        type: 'POST',
        data: {
            Id: $("#EditExpenseId").val(),
            Category: $("#EditExpenseCategory").val(),
            Amount: $("#EditAmount").val(),
            Type: $("#EditExpenseType").val(),
            ExpenseDate: $("#EditExpenseDate").val(),
            Note: $("#EditNote").val()
        },
        headers: {
            'RequestVerificationToken':
                $('#antiForgeryForm input[name="__RequestVerificationToken"]').val()
        },
        success: function (response) {
            closeExpenseDrawer();
            showToast("Action Update Successful");
            loadExpense(1);
        },
        error: function () {
            showToast("Action Update Failed", "error");
        }
    });
}

function confirmDeleteExpense(id) {
    openConfirmModal(
        'Do you want to delete this expense?',
        function () {
            deleteExpense(id);
        },
        'Delete Expense'
    );
}

function deleteExpense(id) {

    $.ajax({
        url: '/Admin/Cost/Delete',
        type: 'POST',
        data: {
            Id: id,
        },
        headers: {
            'RequestVerificationToken':
                $('#antiForgeryForm input[name="__RequestVerificationToken"]').val()
        },
        success: function (response) {
            showToast("Action Delete Expense Successful");
            loadExpense(1);
        },
        error: function () {
            showToast("Action Delete Expense Failed", "error");
        }
    });
}

function openExpenseDrawer() {
    $("#expenseDrawer").addClass("active");
}

function closeExpenseDrawer() {
    $("#expenseDrawer").removeClass("active");
}

function loadExpense(page = 1) {

    const year = $('#yearFilter').val() || 0;
    const month = $('#monthFilter').val() || 13;
    $.ajax({
        url: "/Admin/Cost/LoadExpense",
        type: "GET",
        data: {
            page: page,
            year: year,
            month: month
        },
        success: function (html) {

            $("#expenseContent").html(html);

            requestAnimationFrame(() => {
                loadExpenseChart(year);
                loadExpenseBreakdownChart(month, year);

            });
        },
        error: function () {
            showToast("Failed to load expense data", "error");
        }
    });
}

function loadProfit() {

    const year = $('#yearProfitFilter').val() || 0;
    $.ajax({
        url: "/Admin/Cost/LoadProfit",
        type: "GET",
        data: {
            year: year
        },
        success: function (html) {
            $("#profitContent").html(html);
            loadComparedChart(year);
            loadProfitTrendChart(year);
        },
        error: function () {
            showToast("Failed to load profit data", "error");
        }
    });
}


function openExpenseModal() {
    $("#expenseModal").addClass("show");
}

function closeExpenseModal() {
    $("#expenseModal").removeClass("show");

}
$(document).on('click', '.modalexpense-overlay', function () {
    closeExpenseModal();
});
function addExpense() {
    const URL = "/Admin/Cost/NewExpense";
    const btn = $(".btn.primary");

    btn.prop("disabled", true);
    $.ajax({
        url: URL,
        type: "POST",
        data: {
            Category: $("#ExpenseCategory").val(),
            Amount: $("#Amount").val(),
            Type: $("#ExpenseType").val(),
            ExpenseDate: $("#ExpenseDate").val(),
            Note: $("#Note").val(),
            __RequestVerificationToken:
                $('#antiForgeryForm input[name="__RequestVerificationToken"]').val()
        },
        success: function () {
            showToast("Add Expense Successfully", "success");
            closeExpenseModal();
            loadExpense(1);
        },
        error: function () {
            showToast("Action failed", "error");
        },
        complete: function () {
            btn.prop("disabled", false);
        }
    });
}

/* ===========================
   FILTER CHANGE EVENT
=========================== */

$(document).on('change', '#yearFilter, #monthFilter', function () {
    loadExpense(1);
});

$(document).on('change', '#yearProfitFilter', function () {
    loadProfit();
});

