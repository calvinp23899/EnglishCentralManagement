$(document).ready(function () {
    loadRevenueChart();
});
function loadRevenueChart() {
    const ctx = document.getElementById('revenueChart');
    if (!ctx) return;
    const route = '/Admin/Home/RevenueChart';
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
    new Chart(ctx, {
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


//document.addEventListener("DOMContentLoaded", function () {
//    const ctx = document.getElementById('revenueChart');
//    if (!ctx || !window.revenueChartData) return;

//    new Chart(ctx, {
//        type: 'bar',
//        data: {
//            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
//            datasets: [{
//                label: 'Revenue',
//                data: window.revenueChartData,
//                backgroundColor: '#3b82f6',
//                borderRadius: 8
//            }]
//        },
//        options: {
//            responsive: true,
//            maintainAspectRatio: false,
//            plugins: {
//                legend: { display: false },
//                tooltip: {
//                    callbacks: {
//                        label: ctx =>
//                            (ctx.raw / 1_000_000).toFixed(0) + 'M'
//                    }
//                }
//            },
//            scales: {
//                y: {
//                    beginAtZero: true,
//                    ticks: {
//                        callback: value => (value / 1_000_000) + 'M'
//                    }
//                }
//            }
//        }
//    });
//});

