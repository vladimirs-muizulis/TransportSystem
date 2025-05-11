document.addEventListener("DOMContentLoaded", function () {
    const modeTransportButton = document.getElementById("modeTransport");
    const modeRoutesButton = document.getElementById("modeRoutes");
    const listTitle = document.getElementById("listTitle");
    const formTitle = document.getElementById("formTitle");
    const itemList = document.getElementById("itemList");
    const editForm = document.getElementById("editForm");
    const transportFields = document.getElementById("transportFields");
    const routeFields = document.getElementById("routeFields");
    const assignedBusSelect = document.getElementById("assignedBus");
    const addNewButton = document.getElementById("addNewButton");

    let currentMode = "transport";

    modeTransportButton.addEventListener("click", () => switchMode("transport"));
    modeRoutesButton.addEventListener("click", () => switchMode("routes"));

    addNewButton.addEventListener("click", () => {
        editForm.reset();
        delete editForm.dataset.id;
        editForm.dataset.type = currentMode === "transport" ? "transport" : "route";

        formTitle.textContent = currentMode === "transport" ? "Add Transport" : "Add Route";

        transportFields.style.display = currentMode === "transport" ? "block" : "none";
        routeFields.style.display = currentMode === "routes" ? "block" : "none";

        if (currentMode === "routes") {
            loadAssignedBuses();
        }
    });

    function switchMode(mode) {
        currentMode = mode;

        if (mode === "transport") {
            modeTransportButton.classList.add("active");
            modeRoutesButton.classList.remove("active");
            listTitle.textContent = "Transport List";
            formTitle.textContent = "Edit Transport";
            transportFields.style.display = "block";
            routeFields.style.display = "none";
            loadTransports();
        } else {
            modeTransportButton.classList.remove("active");
            modeRoutesButton.classList.add("active");
            listTitle.textContent = "Route List";
            formTitle.textContent = "Edit Route";
            transportFields.style.display = "none";
            routeFields.style.display = "block";
            loadRoutes();
            loadAssignedBuses();
        }
    }

    async function loadTransports() {
        const response = await fetch("/api/Transport");
        const transports = await response.json();
        renderList(transports, "transport");
    }

    async function loadRoutes() {
        const response = await fetch("/api/BusRoute");
        const routes = await response.json();
        renderList(routes, "route");
    }

    async function loadAssignedBuses() {
        const response = await fetch("/api/Transport");  // Get all buses
        const buses = await response.json();

        assignedBusSelect.innerHTML = '<option value="">-- Not Assigned --</option>'; // Add "Not Assigned" option
        buses.forEach(bus => {
            const option = document.createElement("option");
            option.value = bus.id;
            option.textContent = `${bus.name} (${bus.type})`; // Show bus name and type
            assignedBusSelect.appendChild(option);
        });
    }

    function renderList(items, type) {
        const itemList = document.getElementById("itemList");
        itemList.innerHTML = "";

        items.forEach(item => {
            const li = document.createElement("li");
            li.classList.add("list-item"); // For styling convenience
            li.style.display = "flex";
            li.style.justifyContent = "space-between";
            li.style.alignItems = "center";

            // Block with the name
            const textDiv = document.createElement("div");
            textDiv.classList.add("route-name");
            textDiv.textContent = type === "transport" ? `${item.name} (${item.type})` : `${item.name}`;

            // Button group
            const buttonGroup = document.createElement("div");
            buttonGroup.classList.add("button-group");
            buttonGroup.style.display = "flex";
            buttonGroup.style.gap = "10px";

            // "Edit" button
            const editButton = document.createElement("button");
            editButton.textContent = "Edit";
            editButton.classList.add("edit-button");
            editButton.addEventListener("click", () => populateForm(item, type));

            // "Delete" button
            const deleteButton = document.createElement("button");
            deleteButton.textContent = "Delete";
            deleteButton.classList.add("delete-button");
            deleteButton.addEventListener("click", () => deleteItem(item, type));

            // Insert buttons into the group
            buttonGroup.appendChild(editButton);
            buttonGroup.appendChild(deleteButton);

            // Insert into li
            li.appendChild(textDiv);
            li.appendChild(buttonGroup);

            itemList.appendChild(li);
        });
    }

    async function deleteItem(item, type) {
        const confirmDelete = confirm("Are you sure you want to delete this item?");
        if (!confirmDelete) return;

        let url;
        if (type === "transport") {
            url = `/api/Transport/${item.id}`;
        } else if (type === "route") {
            url = `/api/BusRoute/${item.id}`;
        }

        try {
            const response = await fetch(url, {
                method: "DELETE",
                headers: { "Content-Type": "application/json" },
            });

            if (response.ok) {
                alert("Item deleted!");
                // Reload the list after deletion
                if (type === "transport") {
                    await loadTransports();
                } else if (type === "route") {
                    await loadRoutes();
                }
            } else {
                alert("Error deleting the item.");
            }
        } catch (error) {
            console.error("Error deleting:", error);
            alert("An error occurred while deleting.");
        }
    }

    async function populateForm(item, type) {
        editForm.reset();

        if (type === "transport") {
            editForm.name.value = item.name;
            editForm.type.value = item.type;
            editForm.capacity.value = item.capacity;
            formTitle.textContent = "Edit Transport";
        } else {
            editForm.routeName.value = item.name;
            editForm.stops.value = item.stops.map((s) => s.location).join(", ");
            editForm.arrivalTimes.value = item.stops.map((s) => s.arrivalTime || s.arrival || "").join(", ");

            formTitle.textContent = "Edit Route";

            await loadAssignedBuses(); // Wait for buses to load
            editForm.assignedBus.value = item.assignedBusId || "";
        }

        editForm.dataset.id = item.id;
        editForm.dataset.type = type;
    }

    editForm.addEventListener("submit", async function (e) {
        e.preventDefault();
        const id = this.dataset.id;
        const type = this.dataset.type;

        if (type === "transport") {
            const name = this.name.value.trim();
            const transportType = this.type.value.trim();
            const capacityValue = parseInt(this.capacity.value, 10);

            if (!name || !transportType || isNaN(capacityValue)) {
                alert("Please fill in all fields correctly.");
                return;
            }

            const transportData = { name, type: transportType, capacity: capacityValue };
            if (id) transportData.id = parseInt(id);

            const method = id ? "PUT" : "POST";
            const url = id ? `/api/Transport/${id}` : `/api/Transport`;

            const response = await fetch(url, {
                method,
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(transportData)
            });

            if (!response.ok) {
                alert("Error saving transport");
                return;
            }

            await loadTransports();
        } else if (type === "route") {
            const routeName = this.routeName.value.trim();
            const stopList = this.stops.value.split(",").map(s => s.trim()).filter(s => s);
            const timeList = this.arrivalTimes.value.split(",").map(t => t.trim()).filter(t => t);
            const assignedBusId = parseInt(this.assignedBus.value, 10);

            // Check for matching number of stops and times
            if (!routeName || stopList.length === 0 || stopList.length !== timeList.length) {
                alert("Make sure all fields are filled and the number of stops matches the arrival times.");
                return;
            }

            const stops = stopList.map((location, index) => ({
                location,
                arrivalTime: timeList[index],
                departureTime: timeList[index],
                order: index
            }));

            const routeData = { name: routeName, assignedBusId, stops };
            if (id) routeData.id = parseInt(id);

            const method = id ? "PUT" : "POST";
            const url = id ? `/api/BusRoute/${id}` : `/api/BusRoute`;

            const response = await fetch(url, {
                method,
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(routeData)
            });

            if (!response.ok) {
                alert("Error saving route");
                return;
            }

            await loadRoutes();
        }

        alert("Changes saved!");
        editForm.reset();
        delete editForm.dataset.id;
    });

    loadTransports(); // Start with the transport mode
});
