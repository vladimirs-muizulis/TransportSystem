$(document).ready(function () {
    loadTransport();
});

function loadTransport() {
    $.get("/api/Transport", function (data) {
        renderTable(data);
    });
}

function renderTable(data) {
    var rows = "";
    data.forEach(function (item) {
        rows += `<tr>
                    <td>${item.id}</td>
                    <td><input type="text" value="${item.name}" data-id="${item.id}" class="name-field" /></td>
                    <td><input type="text" value="${item.type}" data-id="${item.id}" class="type-field" /></td>
                    <td><input type="number" value="${item.capacity}" data-id="${item.id}" class="capacity-field" /></td>
                    <td>
                        <button onclick="updateTransport(${item.id})" class="btn">Update</button>
                        <button onclick="deleteTransport(${item.id})" class="btn btn-danger">Delete</button>
                    </td>
                 </tr>`;
    });
    $("#transportTable tbody").html(rows);
}

function addTransport() {
    var transport = {
        name: $("#transportName").val(),
        type: $("#transportType").val(),
        capacity: parseInt($("#transportCapacity").val())
    };

    $.ajax({
        url: "/api/Transport",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(transport),
        success: function () {
            loadTransport();
            $("#transportName").val("");
            $("#transportType").val("");
            $("#transportCapacity").val("");
        },
        error: function () {
            alert("Failed to add transport.");
        }
    });
}

function searchTransport() {
    var type = $("#searchType").val();
    if (type.trim() === "") {
        alert("Please enter a transport type.");
        return;
    }

    $.get(`/api/Transport/search?type=${type}`, function (data) {
        renderTable(data);
    }).fail(function () {
        alert("Failed to find transport with that type.");
    });
}

function updateTransport(id) {
    var name = $(`.name-field[data-id='${id}']`).val();
    var type = $(`.type-field[data-id='${id}']`).val();
    var capacity = parseInt($(`.capacity-field[data-id='${id}']`).val());

    var transport = {
        id: id,
        name: name,
        type: type,
        capacity: capacity
    };

    $.ajax({
        url: `/api/Transport/${id}`,
        type: "PUT",
        contentType: "application/json",
        data: JSON.stringify(transport),
        success: function () {
            loadTransport();
        },
        error: function () {
            alert("Failed to update transport.");
        }
    });
}

function deleteTransport(id) {
    if (!confirm("Are you sure you want to delete this transport?")) {
        return;
    }

    $.ajax({
        url: `/api/Transport/${id}`,
        type: "DELETE",
        success: function () {
            loadTransport();
        },
        error: function () {
            alert("Failed to delete transport.");
        }
    });
}