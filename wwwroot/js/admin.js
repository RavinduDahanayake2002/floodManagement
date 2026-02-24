window.adminDashboard = {
    map: null,
    heatLayer: null,
    chart: null,

    initMapAndHeatmap: function (elementId, heatPoints) {
        if (this.map) {
            this.map.remove();
        }

        // Initialize map centered on Sri Lanka
        this.map = L.map(elementId).setView([7.8731, 80.7718], 7);

        // Add standard OSM tile layer
        L.tileLayer('https://{s}.basemaps.cartocdn.com/dark_all/{z}/{x}/{y}{r}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors &copy; <a href="https://carto.com/attributions">CARTO</a>',
            subdomains: 'abcd',
            maxZoom: 19
        }).addTo(this.map);

        // Map the points (Lat, Lng, intensity). Assuming heatPoints is [[lat, lng, 1], ...]
        if (heatPoints && heatPoints.length > 0) {
            this.heatLayer = L.heatLayer(heatPoints, {
                radius: 25,
                blur: 15,
                maxZoom: 17,
                gradient: { 0.4: 'yellow', 0.65: 'orange', 1: 'red' }
            }).addTo(this.map);
        }
    },

    initStatusChart: function (elementId, labels, data) {
        const ctx = document.getElementById(elementId).getContext('2d');
        
        if (this.chart) {
            this.chart.destroy();
        }

        this.chart = new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: labels,
                datasets: [{
                    data: data,
                    backgroundColor: [
                        '#facc15', // Pending
                        '#38bdf8', // Approved
                        '#34d399', // Processed
                        '#f87171'  // Rejected
                    ],
                    borderWidth: 0,
                    hoverOffset: 4
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'right',
                        labels: {
                            color: '#94a3b8'
                        }
                    }
                },
                cutout: '70%'
            }
        });
    },
    
    downloadData: function (filename, contentType, content) {
        const file = new Blob([content], {type: contentType});
        const a = document.createElement("a");
        const url = URL.createObjectURL(file);
        a.href = url;
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        setTimeout(function() {
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);
        }, 0);
    }
};
