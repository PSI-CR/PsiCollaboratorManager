
$(document).ready(function () {
    $("#grid").jqGrid({
        url: '/Collaborator/GetSchedule',
        datatype: 'json',
        colModel: [
            { label: 'Día', name: 'DayName', width: 200 },
            {
                label: 'Hora Inicio',
                name: 'BeginTime',
                width: 150,
                formatter: function (cellValue) {
                    return formatTime(cellValue);
                }
            },
            {
                label: 'Hora Final',
                name: 'EndTime',
                width: 150,
                formatter: function (cellValue) {
                    return formatTime(cellValue);
                }
            }
        ],
        viewrecords: true,
        height: 'auto',
        rowNum: 1000, 
        pager: "",
        jsonReader: {
            root: 'rows',
            repeatitems: false
        },
        loadError: function (error) {
            console.error('Error:', error);
        }
    });

    function formatTime(timeObj) {
        if (timeObj && timeObj.Hours !== undefined && timeObj.Minutes !== undefined) {
            const hours = timeObj.Hours || 0;
            const minutes = timeObj.Minutes || 0;
            return `${("0" + hours).slice(-2)}:${("0" + minutes).slice(-2)}`;
        }
        return '';
    }

    $('#selectSchedule').change(function () {
        var selectedId = $('#selectSchedule').val();
        if (selectedId == "0") {
            $('#grid').hide(); 
            return;
        }
        $('#grid').show(); 
        LoadSchedule(selectedId);
    });

    function LoadSchedule(selectedId) {
        $("#grid").jqGrid('setGridParam', {
            url: '/Collaborator/GetSchedule',
            datatype: 'json',
            postData: { scheduleId: selectedId },
            page: 1 
        }).trigger('reloadGrid');
    }
});

$('#btnAssign').on('click', function () {
    var collaboratorIds = $('#select2').find('option').map(function () {
        return this.value;
    }).get();
    var scheduleId = $('#selectSchedule').val();
    if (collaboratorIds.length > 0 && scheduleId) AssingCollaborators(collaboratorIds, scheduleId);
    else
        new Messi(
            'Seleccione al menos un colaborador y un horario antes de asignar.',
            { title: 'Seleccione un Colaborador', titleClass: 'anim warning'}
        );    
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
                    new Messi(
                        result.message,
                        { title: 'Exito', titleClass: 'anim success' }
                    );
                }
                else
                    new Messi(
                    result.message,
                    { title: 'Error', titleClass: 'anim error'}
                );                    ;
            },
            error: function (xhr, status, error)
            {
                new Messi(
                    error,
                    { title: 'Error', titleClass: 'anim error'}
                );
            }
        });
    }
    else {
         new Messi(
            'Favor seleccione una fila de datos.',
            { title: 'Error', titleClass: 'anim error'}
        );
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
                new Messi(
                    'Horarios asignados correctamente.',
                    { title: 'Exito', titleClass: 'anim success' }
                );
                $('#select2').empty();
                RefreshGrid();
            }
            else
                new Messi(
                    'Error al asignar colaboradores: ' + result.message,
                    { title: 'Error', titleClass: 'anim error'}
                );              
        },
        error: function (xhr, status, error) {
            new Messi(
                '.',
                { title: 'Seleccione un Colaborador', titleClass: 'anim warning' }
            );           
        }
    });
}
function RefreshGrid() {
    $("#jqGrid").jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}
$(document).ready(function () {
    function loadGrid(selectedId) {
        $("#grid").jqGrid({
            url: '/Collaborator/GetSchedule', 
            type: 'POST',
            postData: { scheduleId: selectedId },
            datatype: 'json',
            colModel: [
                { label: 'Día', name: 'ScheduleDayName', width: 200 },
                { label: 'Hora Inicio', name: 'BeginTime', width: 150 },
                { label: 'Hora Final', name: 'EndTime', width: 150 }
            ],
            viewrecords: true,
            height: 'auto',
            rowNum: 10,
            pager: "#pager",
            jsonReader: {
                root: function (data) {
                    return data;
                },
                repeatitems: false
            },
            loadError: function (xhr, status, error) {
                console.error('Error:', error);
            }
        });
    }

    $('#selectSchedule').change(function () {
        var selectedId = $('#selectSchedule').val();
        if (selectedId == "0") {
            $('#jqGridContainer').hide();
            return;
        }
        $('#jqGridContainer').show();
        loadGrid(selectedId);
    });
});