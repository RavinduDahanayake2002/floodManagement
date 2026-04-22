window.renderHistoryChart = (canvasId, dataLabels, dataValues) => {
    const ctx = document.getElementById(canvasId);
    if (!ctx) return;

    // Destroy previous chart if it exists
    if (window.historyChartInstance) {
        window.historyChartInstance.destroy();
    }

    window.historyChartInstance = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: dataLabels,
            datasets: [{
                label: 'Flood Events',
                data: dataValues,
                backgroundColor: 'rgba(0, 178, 170, 0.5)',
                borderColor: 'rgba(0, 178, 170, 1)',
                borderWidth: 1,
                borderRadius: 4
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: false }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        stepSize: 1,
                        color: 'rgba(255, 255, 255, 0.7)'
                    },
                    grid: { color: 'rgba(255, 255, 255, 0.1)' }
                },
                x: {
                    ticks: { color: 'rgba(255, 255, 255, 0.7)' },
                    grid: { display: false }
                }
            }
        }
    });
};
