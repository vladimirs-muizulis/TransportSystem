document.addEventListener("DOMContentLoaded", () => {
    const routeList = document.getElementById("userRouteList");
    const fromInput = document.getElementById("fromInput");
    const toInput = document.getElementById("toInput");
    const searchForm = document.getElementById("searchForm");

    let allRoutes = [];

    // Load all routes from the server
    async function loadRoutes() {
        try {
            const response = await fetch("/api/BusRoute");
            allRoutes = await response.json();
            renderRoutes(allRoutes);
        } catch (error) {
            routeList.innerHTML = "<p>Failed to load routes.</p>";
        }
    }

    function renderRoutes(routes) {
        routeList.innerHTML = "";

        if (routes.length === 0) {
            routeList.innerHTML = "<p>No routes found.</p>";
            return;
        }

        routes.forEach(route => {
            const card = document.createElement("div");
            card.className = "route-card";

            const header = document.createElement("div");
            header.className = "route-header";

            const title = document.createElement("h3");
            title.textContent = route.name;

            const transportInfo = document.createElement("span");
            transportInfo.textContent = route.assignedBusName || "No vehicle assigned";

            header.appendChild(title);
            header.appendChild(transportInfo);
            card.appendChild(header);

            const stopsList = document.createElement("ul");
            stopsList.className = "stops-list";

            route.stops.forEach(stop => {
                const li = document.createElement("li");
                li.textContent = `${stop.location} – arrival: ${stop.arrival}`;
                stopsList.appendChild(li);
            });

            card.appendChild(stopsList);

            // Toggle stop list visibility on header click
            header.addEventListener("click", () => {
                stopsList.classList.toggle("active");
            });

            routeList.appendChild(card);
        });
    }

    searchForm.addEventListener("submit", (e) => {
        e.preventDefault();

        const from = fromInput.value.trim().toLowerCase();
        const to = toInput.value.trim().toLowerCase();

        // If either input is empty, reset to full list
        if (!from || !to) {
            renderRoutes(allRoutes);
            return;
        }

        // Filter routes that include both 'from' and 'to' in correct order
        const filtered = allRoutes.filter(route => {
            const stops = route.stops.map(s => s.location.toLowerCase());
            const fromIndex = stops.findIndex(s => s.includes(from));
            const toIndex = stops.findIndex(s => s.includes(to));
            return fromIndex >= 0 && toIndex > fromIndex;
        });

        renderRoutes(filtered);
    });

    // Initial route loading
    loadRoutes();
});
