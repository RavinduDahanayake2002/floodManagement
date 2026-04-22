export function initMap(elementId, center, zoom, markers) {
    const map = L.map(elementId, {
        zoomControl: false
    }).setView([center.lat, center.lng], zoom);

    L.control.zoom({
        position: 'bottomright'
    }).addTo(map);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
        maxZoom: 19
    }).addTo(map);

    if (markers) {
        markers.forEach(m => {
            L.marker([m.lat, m.lng]).addTo(map)
                .bindPopup(m.popup);
        });
    }

    // Use ResizeObserver to ensure map checks its size whenever the container changes
    const resizeObserver = new ResizeObserver(() => {
        map.invalidateSize();
    });
    resizeObserver.observe(document.getElementById(elementId));

    // Store observer on map object to disconnect later if needed (optional but good practice)
    map._resizeObserver = resizeObserver;

    return map;
}

export function addGeoJson(map, geoJsonData, styleCallbackName) {
    L.geoJSON(geoJsonData, {
        style: function (feature) {
            // Simple style mapping based on properties
            var risk = feature.properties.riskLevel;
            var color = '#00BCD4'; // Low (Cyan)
            if (risk === 'High') color = '#EF4444';
            if (risk === 'Medium') color = '#F59E0B';

            return { color: color, weight: 2, fillOpacity: 0.4 };
        }
    }).addTo(map);
}

export function setView(map, lat, lng, zoom) {
    map.setView([lat, lng], zoom);
}

export function addClickEvent(map, dotNetHelper) {
    map.on('click', function (e) {
        dotNetHelper.invokeMethodAsync('OnMapClick', { lat: e.latlng.lat, lng: e.latlng.lng });
    });
}

export function addMarker(map, lat, lng) {
    if (window.currentMarker) {
        map.removeLayer(window.currentMarker);
    }
    window.currentMarker = L.marker([lat, lng]).addTo(map);
}

export function addRiskRadius(map, lat, lng, riskLevel) {
    if (window.currentRiskCircle) {
        map.removeLayer(window.currentRiskCircle);
    }
    if (window.currentRiskPulse) {
        map.removeLayer(window.currentRiskPulse);
    }

    var color = '#00BCD4'; // Low (Cyan)
    var pulseColor = 'rgba(0, 188, 212, 0.6)';

    if (riskLevel === 'High') {
        color = '#EF4444'; // Red
        pulseColor = 'rgba(239, 68, 68, 0.6)';
    } else if (riskLevel === 'Medium') {
        color = '#F59E0B'; // Yellow/Orange
        pulseColor = 'rgba(245, 158, 11, 0.6)';
    }

    // A static circle filling the division area (~2.5km radius)
    window.currentRiskCircle = L.circle([lat, lng], {
        color: color,
        fillColor: color,
        fillOpacity: 0.15,
        weight: 1.5,
        radius: 2500
    }).addTo(map);

    // A custom marker that acts as the "blast"/pulse animation center
    const pulseIcon = L.divIcon({
        className: 'risk-pulse-icon',
        html: `<div style="--pulse-color: ${pulseColor}; width: 24px; height: 24px; border-radius: 50%; background-color: ${color}; outline: 2px solid #00BCD4; box-shadow: 0 0 0 0 var(--pulse-color); animation: mapPulse 2s infinite;"></div>`,
        iconSize: [24, 24],
        iconAnchor: [12, 12]
    });

    window.currentRiskPulse = L.marker([lat, lng], { icon: pulseIcon }).addTo(map);
}

export function addShelterMarker(map, lat, lng, popupContent) {
    if (!window.shelterMarkers) window.shelterMarkers = [];

    const shelterIcon = L.divIcon({
        className: 'custom-div-icon',
        html: "<div style='background-color:#00B2AA;width:24px;height:24px;border-radius:50%;border:2px solid #00BCD4;display:flex;align-items:center;justify-content:center;box-shadow:0 0 10px rgba(0, 188, 212, 0.4);'><span style='font-size:12px;'>🏥</span></div>",
        iconSize: [24, 24],
        iconAnchor: [12, 12]
    });

    const marker = L.marker([lat, lng], { icon: shelterIcon }).addTo(map);
    if (popupContent) {
        marker.bindPopup(popupContent);
    }
    window.shelterMarkers.push(marker);
}

// SLIC Agent Locator Functions
export function addAgentMarker(map, lat, lng, type, popupContent) {
    if (!window.agentMarkers) window.agentMarkers = [];

    const isBranch = type === 'Branch';
    const color = isBranch ? '#009688' : '#00BCD4'; // Dark Teal for Branch, Cyan for Agent
    const iconStr = isBranch ? '🏢' : '👤';

    const agentIcon = L.divIcon({
        className: 'custom-div-icon',
        html: `<div style='background-color:${color};width:28px;height:28px;border-radius:50%;border:2px solid #00BCD4;display:flex;align-items:center;justify-content:center;box-shadow:0 0 12px rgba(0, 188, 212, 0.4); text-shadow: none;'><span style='font-size:14px;'>${iconStr}</span></div>`,
        iconSize: [28, 28],
        iconAnchor: [14, 14]
    });

    const marker = L.marker([lat, lng], { icon: agentIcon }).addTo(map);
    if (popupContent) {
        marker.bindPopup(popupContent);
    }
    window.agentMarkers.push(marker);
}

export function addUserLocation(map, lat, lng) {
    if (window.userMarker) {
        map.removeLayer(window.userMarker);
    }

    const userIcon = L.divIcon({
        className: 'user-pulse-icon',
        html: `<div style="width: 16px; height: 16px; border-radius: 50%; background-color: #00BCD4; outline: 3px solid #009688; box-shadow: 0 0 10px rgba(0, 188, 212, 0.7); animation: mapPulse 2s infinite;"></div>`,
        iconSize: [16, 16],
        iconAnchor: [8, 8]
    });

    window.userMarker = L.marker([lat, lng], { icon: userIcon }).addTo(map);
    map.flyTo([lat, lng], 10, { animate: true, duration: 1.5 });
}

export function drawRouteLine(map, lat1, lng1, lat2, lng2) {
    if (window.routeLine) {
        map.removeLayer(window.routeLine);
    }

    var latlngs = [
        [lat1, lng1],
        [lat2, lng2]
    ];

    window.routeLine = L.polyline(latlngs, {
        color: '#00B2AA',
        weight: 3,
        dashArray: '5, 10',
        opacity: 0.8
    }).addTo(map);
}

export function clearAgentMap(map) {
    if (window.agentMarkers) {
        window.agentMarkers.forEach(m => map.removeLayer(m));
        window.agentMarkers = [];
    }
    if (window.userMarker) {
        map.removeLayer(window.userMarker);
        window.userMarker = null;
    }
    if (window.routeLine) {
        map.removeLayer(window.routeLine);
        window.routeLine = null;
    }
}
