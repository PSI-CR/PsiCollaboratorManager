$('#selectSchedule').change(function () {
    var selectedId = $('#selectSchedule').val();
    if (selectedId == "0") {
        document.getElementById("tableScheduleInfo").style.display = "none";
        return;
    }
    LoadSchedule(selectedId);
});
$('#btnAssign').on('click', function () {
    var collaboratorIds = $('#select2').find('option').map(function () {
        return this.value;
    }).get();
    var scheduleId = $('#selectSchedule').val();
    if (collaboratorIds.length > 0 && scheduleId) AssingCollaborators(collaboratorIds, scheduleId);
    else alert('Seleccione al menos un colaborador y un horario antes de asignar.');
});

function DismissSchedule() {

    var grid = $("#jqGrid");
    var rowKey = grid.getGridParam("selrow");
    if (rowKey) {

        var key = $("#jqGrid").jqGrid('getCell', rowKey, 'CollaboratorId');
        $.ajax({
            url: '/Collaborator/DismissSchedule',
            type: 'POST',
            dataType: 'json',
            data: { collaboratorId: key },
            success: function (result) {
                if (result.success) {
                    RefreshGrid();
                    let select = $('#select1');
                    select.empty();
                    result.collaborators.forEach((collaborator, index) => {
                        select.append(new Option(collaborator.Text, collaborator.Value));
                    });
                    alert(result.message);
                }
                else alert(result.message);
            },
            error: function (xhr, status, error) { alert(error) }
        });
    }
    else {
        alert("No rows are selected");
    }
}
function AssingCollaborators(collaboratorIds, scheduleId) {
    $.ajax({
        url: '/Collaborator/AssignSchedule',
        type: 'POST',
        dataType: 'json',
        data: { collaboratorIds: collaboratorIds, scheduleId: scheduleId },
        success: function (result) {
            $('#mensaje').text(result.message);
            if (result.success) {
                alert('Colaboradores asignados correctamente.');
                $('#select2').empty();
                RefreshGrid();
            }
            else alert('Error al asignar colaboradores: ' + result.message);
        },
        error: function (xhr, status, error) { alert('Error al asignar colaboradores: ' + error) }
    });
}
function RefreshGrid() {
    $("#jqGrid").jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}
function LoadSchedule(selectedId) {
    $.ajax({
        url: '/Collaborator/GetSchedule',
        type: 'POST',
        data: { scheduleId: selectedId },
        success: function (response) {
            var tableBody = document.getElementById("tableScheduleInfo");
            var tableBodyRows = document.getElementById("tableBody");
            var tableBodyAs = $("tableScheduleInfo");
            tableBody.style.display = "table";
            tableBodyAs.empty();
            tableBodyRows.remove();
            const formatNumber = n => ("0" + n).slice(-2);
            var newTableBody = document.createElement("tbody");
            newTableBody.setAttribute("id", "tableBody");
            tableBody.appendChild(newTableBody);
            tableBody = newTableBody;
            response.forEach((scheduleDaily, i) => {
                var row = document.createElement("tr");
                var dayNameCell = document.createElement("td");
                dayNameCell.textContent = scheduleDaily.ScheduleDayName;
                row.appendChild(dayNameCell);
                var beginTimeCell = document.createElement("td");
                let beginTime = new Date(parseInt(scheduleDaily.BeginTime.match(/\/Date\((\d+)\)\//)[1], 10));
                let endTime = new Date(parseInt(scheduleDaily.EndTime.match(/\/Date\((\d+)\)\//)[1], 10));
                beginTimeCell.textContent = beginTime.getHours() + ":" + formatNumber(beginTime.getMinutes());
                row.appendChild(beginTimeCell);
                var endTimeCell = document.createElement("td");
                endTimeCell.textContent = endTime.getHours() + ":" + formatNumber(endTime.getMinutes());
                row.appendChild(endTimeCell);
                if (i % 2 != 0) row.classList.add('even');
                tableBody.appendChild(row);
            });
        },
        error: function (xhr, status, errorData) {
            console.error(errorData);
        }
    });
}