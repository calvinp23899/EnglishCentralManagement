var expenseBreakdownChart = null;
var expenseChart = null;
var revenueChart = null;
var profitExpenseChart = null;
var profitTrendChart = null;
$(function () {
   loadRevenueChart();
});


//========== CHART REVENUE OVERVIEW TAB ==========
function loadRevenueChart() {
    const ctx = document.getElementById('revenueChart');
    if (!ctx) return;
    const route = '/Admin/Cost/RevenueChart';
    $.ajax({
        url: route,
        type: 'GET',
        success: function (data) {
            drawChart(ctx, data);
        },
        error: function () {
            console.error('Cannot load revenue chart data');
        }
    });
}

function drawChart(ctx, data) {
    if (revenueChart) {
        revenueChart.destroy();
    }
    revenueChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: [
                'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
                'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
            ],
            datasets: [{
                label: 'Revenue',
                data: data,
                backgroundColor: '#3b82f6',
                borderRadius: 8
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: false },
                tooltip: {
                    callbacks: {
                        label: ctx =>
                            (ctx.raw / 1_000_000).toFixed(0) + 'M'
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: value => (value / 1_000_000) + 'M'
                    }
                }
            }
        }
    });
}

//========= CHART EXPENSE 12 MONTHS - EXPENSE TAB =======
function loadExpenseChart(year) {

    const ctx = document.getElementById('expenseBarChart');
    if (!ctx) return;

    $.ajax({
        url: '/Admin/Cost/ExpenseThisYearChart',
        type: 'GET',
        data: { year: year },
        success: function (data) {
            console.log("Expense data:", data);
            drawExpenseChart(ctx, data);
        },
        error: function (err) {
            console.error("Cannot load expense chart data", err);
        }
    });
}

function drawExpenseChart(ctx, data) {
    if (expenseChart) {
        expenseChart.destroy();
    }
    expenseChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: [
                'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
                'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
            ],
            datasets: [{
                label: 'Expense',
                data: data,
                backgroundColor: '#e74c3c',
                borderRadius: 6,
                maxBarThickness: 40
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: false },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            return context.raw.toLocaleString() + " VND";
                        }
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function (value) {
                            return value.toLocaleString();
                        }
                    }
                }
            }
        }
    });
}

//========= CHART EXPENSE BREAKDOWN - EXPENSE TAB =======
function loadExpenseBreakdownChart(month, year) {
    const ctx = document.getElementById('expensePieChart');
    if (!ctx) return;
    const url = '/Admin/Cost/ExpenseBreakdownChart';
    $.ajax({
        url: url,
        type: 'GET',
        data: {
            year: year,
            month: month || null
        },
        success: function (data) {
            drawExpenseBreakdownChart(ctx, data);
        },
        error: function (err) {
            console.error("Cannot load expense breakdown data", err);
        }
    });
}

function drawExpenseBreakdownChart(ctx, data) {
    if (expenseBreakdownChart) {
        expenseBreakdownChart.destroy();
    }
    const centerTextPlugin = {
        id: 'centerText',
        beforeDraw(chart) {
            const { width, height, ctx } = chart;
            ctx.restore();

            const dataset = chart.data.datasets[0].data;
            const total = dataset.reduce((a, b) => a + b, 0);

            const text = total.toLocaleString() + " VND";

            ctx.font = "bold 18px sans-serif";
            ctx.textBaseline = "middle";
            ctx.fillStyle = "#333";

            const textX = Math.round((width - ctx.measureText(text).width) / 2);
            const textY = height / 2;

            ctx.fillText(text, textX, textY);
            ctx.save();
        }
    };
    //Case Data = 0
    const total = data.data.reduce((a, b) => a + b, 0);

    let chartData;
    let backgroundColors;
    let labels;
    if (total === 0) {
        // fake 1 phần tử để chart vẫn render
        chartData = [1];
        labels = ["No Data"];
        backgroundColors = ["#e3e6f0"]; // gray color
    } else {
        chartData = data.data;
        labels = data.labels;
        backgroundColors = [
            '#4e73df',
            '#1cc88a',
            '#36b9cc',
            '#f6c23e',
            '#858796'
        ];
    }
    expenseBreakdownChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                data: chartData,
                backgroundColor: backgroundColors
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            cutout: '65%', // làm lỗ to hơn 
            plugins: {
                legend: {
                    display: total !== 0,
                    position: 'bottom'
                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            let value = context.raw || 0;
                            return value.toLocaleString() + " VND";
                        }
                    }
                }
            }
        },
        plugins: [centerTextPlugin]

    });
}

//===== COMPARED CHART - PROFIT TAB ===========
function loadComparedChart(year) {

    const ctx = document.getElementById('revenueExpenseChart');
    if (!ctx) return;

    $.ajax({
        url: '/Admin/Cost/ComparedChart',
        type: 'GET',
        data: { year: year },
        success: function (data) {
            drawComparedChart(ctx, data);
        },
        error: function (err) {
            console.error("Cannot load compared chart", err);
        }
    });
}
function drawComparedChart(ctx, data) {

    if (profitExpenseChart) {
        profitExpenseChart.destroy();
    }

    const totalRevenue = data.revenueData.reduce((a, b) => a + b, 0);
    const totalExpense = data.expenseData.reduce((a, b) => a + b, 0);

    const allZero = totalRevenue === 0 && totalExpense === 0;

    let labels = data.labels;
    let revenue = data.revenueData;
    let expense = data.expenseData;

    if (allZero) {
        // Fake để chart vẫn render
        labels = ["No Data"];
        revenue = [1];
        expense = [1];
    }

    profitExpenseChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [
                {
                    label: 'Revenue',
                    data: revenue,
                    backgroundColor: '#4e73df'
                },
                {
                    label: 'Expense',
                    data: expense,
                    backgroundColor: '#e74a3b'
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: !allZero,
                    position: 'top'
                },
                tooltip: {
                    enabled: !allZero,
                    callbacks: {
                        label: function (context) {
                            return context.raw.toLocaleString() + " VND";
                        }
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function (value) {
                            return value.toLocaleString();
                        }
                    }
                }
            }
        }
    });
}

//===== TREND PROFIT CHART - PROFIT TAB ===========
function loadProfitTrendChart(year) {
    const ctxProfitTrend = document.getElementById("profitTrendChart");

    if (!ctxProfitTrend) return;
    $.ajax({
        url: "/Admin/Cost/ProfitTrendChart",
        type: "GET",
        data: { year: year },
        success: function (data) {
            drawProfitTrendChart(ctxProfitTrend, data);
        },
        error: function (err) {
            showToast("Failed to load profit trend chart", err);
        }
    });
}

function drawProfitTrendChart(ctx, data) {
    if (profitTrendChart) {
        profitTrendChart.destroy();
    }

    const labelsProfitTrend = data.labels || [];
    const dataProfitTrend = data.profitData || [];

    profitTrendChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labelsProfitTrend,
            datasets: [{
                label: 'Total Profit',
                data: dataProfitTrend,
                borderColor: '#4e73df',
                backgroundColor: 'rgba(78,115,223,0.1)',
                fill: true,
                tension: 0.4,
                pointRadius: 4,
                pointBackgroundColor: '#4e73df'
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    ticks: {
                        callback: function (value) {
                            return value.toLocaleString() + " đ";
                        }
                    }
                }
            },
            plugins: {
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            return context.parsed.y.toLocaleString() + " đ";
                        }
                    }
                }
            }
        }
    });
}