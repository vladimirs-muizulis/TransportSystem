$(document).ready(function () {
    loadTransport();
});

function loadTransport() {
    $.get("/api/Transport", function (data) {
        var rows = "";
        data.forEach(function (item) {
            rows += `<tr>
                        <td>${item.id}</td>
                        <td>${item.name}</td>
                        <td>${item.type}</td>
                        <td>${item.capacity}</td>
                     </tr>`;
        });
        $("#transportTable tbody").html(rows);
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
