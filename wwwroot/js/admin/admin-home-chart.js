document.addEventListener("DOMContentLoaded", function () {
    const ctx = document.getElementById('revenueChart');
    if (!ctx) return;
    
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            datasets: [{
                label: 'Revenue',
                data: [5000, 6200, 7800, 9200, 11000, 13500, 14800, 16200, 17500, 0, 0, 0],
                backgroundColor: '#3b82f6',
                borderRadius: 8
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: { legend: { display: false } },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: value => '$' + value
                    }
                }
            }
        }
    });
});

/*
============= SAU NÀY TẠO 1 API GỌI ĐỂ GET DỮ LIỆU ĐỖ VÀO ===============
document.addEventListener("DOMContentLoaded", async () => {
    const ctx = document.getElementById('revenueChart');
    if (!ctx) return;

    const res = await fetch('/admin/dashboard/revenuebymonth');
    const result = await res.json();

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: result.labels,
            datasets: [{
                data: result.data,
                backgroundColor: '#3b82f6',
                borderRadius: 8
            }]
        },
        options: { responsive: true }
    });
});

*/
