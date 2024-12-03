
$("#jqGrid2").jqGrid({
    url: '',
    mtype: 'GET',
    datatype: 'local',
    colNames: [
        'AttendId', 'CollaboratorId', 'CheckIn', 'CheckOut', 'ValCheckIn', 'Estado CheckIn',
        'ValCheckOut', 'Estado CheckOut', 'Comentario CheckIn', 'Salida', 'Tiempo Total',
        'Horas Extra', 'Horas Pendientes'
    ],
    colModel: [
        { name: 'AttendId', index: 'AttendId', width: 80, align: 'center', hidden: true },
        { name: 'CollaboratorId', index: 'CollaboratorId', width: 80, align: 'center', hidden: true },
        { name: 'CheckIn', index: 'CheckIn', width: 190, align: 'center', formatter: formatDateTimeWithoutSeconds },
        { name: 'CheckOut', index: 'CheckOut', width: 190, align: 'center', formatter: formatDateTimeWithoutSeconds },
        { name: 'CheckInStatus', index: 'CheckInStatus', width: 100, align: 'center', hidden: true },
        { name: 'CheckInStatusWork', index: 'CheckInStatusWork', width: 100, align: 'center' },
        { name: 'CheckOutStatus', index: 'CheckOutStatus', width: 100, align: 'center', hidden: true },
        { name: 'CheckOutStatusWork', index: 'CheckOutStatusWork', width: 100, align: 'center' },
        { name: 'CommentCheckIn', index: 'CommentCheckIn', width: 200, align: 'center', editable: true },
        { name: 'IsOpenCheckIn', index: 'IsOpenCheckIn', width: 100, align: 'center', editable: true },
        { name: 'TotalTime', index: 'TotalTime', width: 120, align: 'center' },
        { name: 'HorasExtra', index: 'HorasExtra', width: 120, align: 'center' },
        { name: 'HorasPendientes', index: 'HorasPendientes', width: 120, align: 'center' },
    ],
    pager: '#jqGridPager2',
    rowNum: 30,
    rowList: [30, 40, 50],
    sortorder: 'asc',
    viewrecords: true,
    gridview: true,
    autoencode: true,
    autowidth: true,
    height: 'auto',
    width: '100%',
    loadonce: true,
    subGrid: true,

    subGridRowExpanded: function (subgridId, rowId) {
        var subgridTableId = subgridId + "_t";
        var rowData = $("#jqGrid2").jqGrid('getRowData', rowId);

        $("#" + subgridId).html("<table id='" + subgridTableId + "' class='scroll'></table>");

        $("#" + subgridTableId).jqGrid({
            datatype: 'local',
            colNames: ['CheckIn', 'CheckOut', 'Estado CheckIn', 'Estado CheckOut', 'Comentario'],
            colModel: [
                { name: 'CheckIn', index: 'CheckIn', width: 150, formatter: formatDateTimeWithoutSeconds },
                { name: 'CheckOut', index: 'CheckOut', width: 150, formatter: formatDateTimeWithoutSeconds },
                { name: 'CheckInStatusWork', index: 'CheckInStatusWork', width: 100 },
                { name: 'CheckOutStatusWork', index: 'CheckOutStatusWork', width: 100 },
                { name: 'CommentCheckIn', index: 'CommentCheckIn', width: 200 }
            ],
            height: '100%',
            autowidth: true,
            rownumbers: true,
            gridview: true,
            viewrecords: true
        });

        // Filtrar registros para el día correspondiente
        var dayRecords = [];
        var rows = $("#jqGrid2").jqGrid('getDataIDs');
        rows.forEach(function (id) {
            var data = $("#jqGrid2").jqGrid('getRowData', id);
            if (data.CheckIn.split(' ')[0] === rowData.CheckIn.split(' ')[0]) {
                dayRecords.push(data);
            }
        });
        // Agregar los datos filtrados al subgrid
        for (var i = 0; i < dayRecords.length; i++) {
            $("#" + subgridTableId).jqGrid('addRowData', i + 1, dayRecords[i]);
        }
    },

    ondblClickRow: function (rowId, iRow, iCol, e) {
        openEditModal(rowId);
    },
    subGridOptions:
    {
        plusicon: "ui-icon-triangle-1-e",
        minusicon: "ui-icon-triangle-1-s",
        openicon: "ui-icon-arrowreturn-1-e"
    },

    loadComplete: function () {
        var grid = $(this);
        var rows = grid.jqGrid('getDataIDs');
        var groupedByDay = new Map();

        var totalWorkedMinutes = 0;
        var totalExtraMinutes = 0;
        var totalMissingMinutes = 0;

        rows.forEach(function (rowId) {
            var rowData = grid.jqGrid('getRowData', rowId);
            var checkInDate = rowData.CheckIn.split(' ')[0];

            if (!groupedByDay.has(checkInDate)) {
                groupedByDay.set(checkInDate, []);
            }
            groupedByDay.get(checkInDate).push(rowId);
        });

        // Promesas globales para todos los días
        var globalPromises = [];

        groupedByDay.forEach(function (rowIds, date) {
            let dayTotalWorkedMinutes = 0;
            let dayTotalExtraMinutes = 0;
            let dayTotalMissingMinutes = 0;

            let dayPromises = rowIds.map(function (rowId) {
                return new Promise(function (resolve) {
                    getScheduleCalculateHoursExtraDaily(rowId, grid)
                        .then(function (result) {
                            // Sumar los minutos por día
                            dayTotalWorkedMinutes += result.totalMinutes;
                            dayTotalExtraMinutes += result.extraMinutes;
                            dayTotalMissingMinutes += result.missingMinutes;

                            // Determinar el color según las condiciones para cada registro
                            if (result.missingMinutes > 0) {
                                grid.jqGrid('setRowData', rowId, false, { 'background-color': '#ffcccc' }); // Rojo
                            } else if (result.extraMinutes > 0) {
                                grid.jqGrid('setRowData', rowId, false, { 'background-color': '#ccffcc' }); // Verde
                            }

                            resolve();
                        })
                        .catch(function (error) {
                            console.log('Error en el cálculo de horas:', error);
                            resolve();
                        });
                });
            });

            let dayPromise = Promise.all(dayPromises).then(function () {
                var dateParts = date.split('/');
                var day = new Date(dateParts[2], dateParts[1] - 1, dateParts[0]);
                var dayId = (day.getDay() + 6) % 7 + 2;

                // Obtener datos de la primera fila del día
                if (rowIds.length > 0) {
                    var firstRowId = rowIds[0];
                    var firstRowData = grid.jqGrid('getRowData', firstRowId);
                    var collaboratorId = firstRowData.CollaboratorId;

                    // Llamar la función AJAX con CollaboratorId
                    consultarHorasExtraPendientes(dayTotalWorkedMinutes, dayId, collaboratorId, firstRowId)
                        .then(function (response) {
                            // Actualizar totales en la primera fila
                            grid.jqGrid('setCell', firstRowId, 'TotalTime', convertirMinutosAHoras(dayTotalWorkedMinutes));
                            grid.jqGrid('setCell', firstRowId, 'HorasExtra', response.data.ExtraTime); // Actualiza con el resultado del AJAX
                            grid.jqGrid('setCell', firstRowId, 'HorasPendientes', response.data.PendingTime); // Actualiza con el resultado del AJAX

                            // Determinar el color de la primera fila
                            if (response.data.PendingTime !== '0h 0m') {
                                grid.jqGrid('setRowData', firstRowId, false, { 'background-color': '#ffcccc' }); // Rojo
                            } else if (response.data.ExtraTime !== '0h 0m') {
                                grid.jqGrid('setRowData', firstRowId, false, { 'background-color': '#ccffcc' }); // Verde
                            }
                        })
                        .catch(function (error) {
                            console.error('Error al consultar horas extra y pendientes:', error);
                        });
                }

                // Ocultar las filas restantes del día
                rowIds.slice(1).forEach(function (rowId) {
                    grid.jqGrid('setRowData', rowId, false, { display: 'none' });
                });
                // Sumar los totales generales
                totalWorkedMinutes += dayTotalWorkedMinutes;
                totalExtraMinutes += dayTotalExtraMinutes;
                totalMissingMinutes += dayTotalMissingMinutes;
            });
            globalPromises.push(dayPromise);
        });

        Promise.all(globalPromises).then(function () {
            actualizarTotalesGenerales(grid);

        });
    }
});

// Function to update general totals
function actualizarTotalesGenerales(grid) {
    // Variables para acumular los minutos
    let totalWorkedMinutes = 0;
    let totalExtraMinutes = 0;
    let totalMissingMinutes = 0;

    // Obtener todas las filas visibles del grid
    const rows = grid.jqGrid('getDataIDs');
    rows.forEach(rowId => {
        const rowData = grid.jqGrid('getRowData', rowId);

        // Convertir los valores de las celdas a minutos
        totalWorkedMinutes += parseMinutesFromFormattedTime(rowData.TotalTime);
        totalExtraMinutes += parseMinutesFromFormattedTime(rowData.HorasExtra);
        totalMissingMinutes += parseMinutesFromFormattedTime(rowData.HorasPendientes);
    });

    // Actualizar los campos HTML con los totales en formato horas:minutos
    $('#totalTimeTextbox').val(convertirMinutosAHoras(totalWorkedMinutes));
    $('#totalExtraTextbox').val(convertirMinutosAHoras(totalExtraMinutes));
    $('#totalFailTextbox').val(convertirMinutosAHoras(totalMissingMinutes));
}

function consultarHorasExtraPendientes(totalMinutes, dayId, collaboratorId, rowId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: '/CheckSchedule/CalculateSchedule',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                totalMinutes: totalMinutes,
                dayId: dayId,
                collaboratorId: collaboratorId
            }),
            success: function (response) {
                if (response.success) {
                    resolve(response);
                } else {
                    reject('Error al obtener los datos: ' + response.message);
                }
            },
            error: function (error) {
                reject('Error en la consulta AJAX: ' + error);
            }
        });
    });
}

//// Función para convertir minutos a formato horas y minutos
function convertirMinutosAHoras(minutos) {
    const horas = Math.floor(minutos / 60);
    const minutosRestantes = minutos % 60;
    return `${horas}h ${minutosRestantes} m`;
}

function parseMinutesFromFormattedTime(formattedTime) {
    if (!formattedTime || formattedTime === '0h 0m') return 0;
    const [hours, minutes] = formattedTime
        .replace('h', '')
        .replace('m', '')
        .trim()
        .split(' ')
        .map(Number);
    return (hours || 0) * 60 + (minutes || 0);
}

$("#jqGrid2").jqGrid('filterToolbar',
    {
        searchOperators: false,
        searchOnEnter: false,
        defaultSearch: "cn"
    });

// Función para abrir el modal y cargar los datos seleccionados
function openEditModal(rowId) {
    var rowData = $("#jqGrid2").jqGrid('getRowData', rowId);
    setModalFields(rowData);
    OpenModal('EditAssistanceModal');
}

// Función para configurar los datos en el modal
function setModalFields(rowData) {
    const [formattedDate, checkInTime] = formatDateTime(rowData.CheckIn);
    const [formattedDateOut, checkOutTime] = formatDateTime(rowData.CheckOut);

    $('#checkin-date').val(formattedDate);
    $('#checkin-time').val(checkInTime);
    $('#checkout-date').val(formattedDateOut);
    $('#checkout-time').val(checkOutTime);
    $('#checkinstatuswork').val(rowData.CheckInStatus);
    $('#checkoutstatuswork').val(rowData.CheckOutStatus);
    $('#commentcheckin').val(rowData.CommentCheckIn);
    $('#attendanceid').val(rowData.AttendId);
    $('#collaboratorid').val(rowData.CollaboratorId);
    $('#isopencheckin').prop('checked', rowData.IsOpenCheckIn === 'Sí');
}

// Función para formatear fecha y hora
function formatDateTime(dateTime) {
    const [date, time] = dateTime.split(' ');
    const [day, month, year] = date.split('/');
    return [`${year}-${month}-${day}`, time];
}

// Función para calcular horas extras y faltantes
function calculateTotalHours(rows, grid) {
    let totalExtraMinutes = 0;
    let totalMissingMinutes = 0;

    rows.forEach(rowId => {
        const { extraMinutes, missingMinutes } = getScheduleCalculateHoursExtraDaily(rowId, grid);
        totalExtraMinutes += extraMinutes;
        totalMissingMinutes += missingMinutes;
    });

    $('#totalExtraHoursTextbox').val(convertirMinutosAHoras(totalExtraMinutes));
    $('#totalMissingHoursTextbox').val(convertirMinutosAHoras(totalMissingMinutes));
}

function getScheduleCalculateHoursExtraDaily(rowId, grid) {
    const rowData = grid.jqGrid('getRowData', rowId);
    const checkInTime = parseDateTime(rowData.CheckIn);
    const checkOutTime = parseDateTime(rowData.CheckOut);

    if (!checkInTime || !checkOutTime) {
        grid.jqGrid('setCell', rowId, 'TotalTime', '0h 0m');
        grid.jqGrid('setCell', rowId, 'HorasExtra', '0h 0m');
        grid.jqGrid('setCell', rowId, 'HorasPendientes', '0h 0m');
        return Promise.resolve({ totalMinutes: 0, extraMinutes: 0, missingMinutes: 0 });
    }

    return getCollaboratorSchedule(rowData.CollaboratorId)
        .then(scheduleWithMinutes => {
            let minutesWorkedForDay = 0;

            scheduleWithMinutes.forEach(schedule => {
                if (schedule.ScheduleDailyId === checkInTime.getDay()) {
                    minutesWorkedForDay = schedule.MinutesWorked;
                }
            });

            const totalMinutes = calculateMinutesBetween(checkInTime, checkOutTime);
            const extraMinutes = totalMinutes > minutesWorkedForDay ? totalMinutes - minutesWorkedForDay : 0;
            const missingMinutes = totalMinutes < minutesWorkedForDay ? minutesWorkedForDay - totalMinutes : 0;

            return { totalMinutes, extraMinutes, missingMinutes };
        })
        .catch(error => {
            console.log('Error:', error);
            return { totalMinutes: 0, extraMinutes: 0, missingMinutes: 0 };
        });
}


// Función para convertir minutos a formato horas y minutos
function convertirMinutosAHoras(minutos) {
    const horas = Math.floor(minutos / 60);
    const minutosRestantes = minutos % 60;
    return `${horas}h ${minutosRestantes} m`;
}


// Función para convertir fecha y hora en formato dd/mm/yyyy hh:mm a un objeto Date
function parseDateTime(dateTimeStr) {
    if (!dateTimeStr) return null;
    const [datePart, timePart] = dateTimeStr.split(' ');
    if (!datePart || !timePart) {
        return null;
    }

    const [day, month, year] = datePart.split('/').map(Number);
    const [hours, minutes] = timePart.split(':').map(Number);

    if (isNaN(day) || isNaN(month) || isNaN(year) || isNaN(hours) || isNaN(minutes)) {
        return null;
    }
    return new Date(year, month - 1, day, hours, minutes);
}

// Función para calcular la diferencia en minutos
function calculateMinutesBetween(startTime, endTime, round = 'down') {
    const diffInMs = endTime - startTime;
    const minutes = diffInMs / 60000;

    if (round === 'up') {
        return Math.ceil(minutes);
    }

    if (round === 'down') {
        return Math.floor(minutes);
    }
    return Math.round(minutes);
}

function formatDateTimeWithoutSeconds(cellValue) {
    if (cellValue) {
        var dateTimeParts = cellValue.split(' ');
        var dateParts = dateTimeParts[0].split('/');
        var timeParts = dateTimeParts[1].split(':');
        var formattedDate = dateParts[0] + '/' + dateParts[1] + '/' + dateParts[2];
        var formattedTime = timeParts[0] + ':' + timeParts[1];
        return formattedDate + ' ' + formattedTime;
    }
    return cellValue;
}

//------------------------------------------------------------------------------------//

function getCollaboratorSchedule(collaboratorId) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: '/CheckSchedule/GetScheduleDaily',
            type: 'POST',
            data: { collaboratorId: collaboratorId },
            success: function (response) {
                if (response.success) {
                    var schedule = response.data;
                    var scheduleWithMinutes = calculateWorkMinutes(schedule);
                    resolve(scheduleWithMinutes);
                } else {
                    alert("No se pudo obtener el horario del colaborador.");
                    reject("Error al obtener el horario");
                }
            },
            error: function (xhr, status, error) {
                console.log("Error al obtener el horario: " + error);
                reject(error);
            }
        });
    });
}

// Función para calcular los minutos trabajados por cada día
function calculateWorkMinutes(schedule) {
    return schedule.map(function (day) {
        // Convertir las fechas de BeginTime y EndTime a objetos Date
        var beginTime = new Date(parseInt(day.BeginTime.replace('/Date(', '').replace(')/', '')));
        var endTime = new Date(parseInt(day.EndTime.replace('/Date(', '').replace(')/', '')));
        var minutesWorked = (endTime - beginTime) / 60000;

        // Retornar el objeto con el ScheduleDailyId y los minutos calculados
        return {
            ScheduleDailyId: day.ScheduleDailyId,
            DayId: day.DayId,
            DayName: day.DayName,
            MinutesWorked: minutesWorked
        };
    });
}

function getDailyTotals(grid) {
    // Obtener todos los registros del grid
    const rows = grid.jqGrid('getRowData');

    // Agrupar los registros por fecha (usamos solo la fecha, no la hora)
    const groupedByDay = {};

    rows.forEach(row => {
        const checkInTime = parseDateTime(row.CheckIn);
        if (!checkInTime) return; // Saltar registros con fechas inválidas
        const dayKey = checkInTime.toISOString().split('T')[0]; // Usamos solo la fecha (YYYY-MM-DD)

        if (!groupedByDay[dayKey]) {
            groupedByDay[dayKey] = {
                totalMinutes: 0,
                extraMinutes: 0,
                missingMinutes: 0,
                rowIds: []
            };
        }

        // Acumular los totales
        const totalMinutes = parseMinutesFromFormattedTime(row.TotalTime);
        const extraMinutes = parseMinutesFromFormattedTime(row.HorasExtra);
        const missingMinutes = parseMinutesFromFormattedTime(row.HorasPendientes);

        groupedByDay[dayKey].totalMinutes += totalMinutes;
        groupedByDay[dayKey].extraMinutes += extraMinutes;
        groupedByDay[dayKey].missingMinutes += missingMinutes;
        groupedByDay[dayKey].rowIds.push(row.id);
    });

    // Actualizar el primer registro de cada día con los totales
    Object.keys(groupedByDay).forEach(dayKey => {
        const group = groupedByDay[dayKey];

        // Obtener el primer registro del día
        const firstRowId = group.rowIds[0];
        grid.jqGrid('setCell', firstRowId, 'TotalTime', convertirMinutosAHoras(group.totalMinutes));
        grid.jqGrid('setCell', firstRowId, 'HorasExtra', convertirMinutosAHoras(group.extraMinutes));
        grid.jqGrid('setCell', firstRowId, 'HorasPendientes', convertirMinutosAHoras(group.missingMinutes));

        // Si hay más registros para el mismo día, ocultarlos
        group.rowIds.slice(1).forEach(rowId => {
            grid.jqGrid('setRowData', rowId, false, { display: 'none' });
        });
    });
}

/*$("#jqGrid2").jqGrid('navGrid', '#jqGridPager2', { edit: false, add: false, del: false });*/
$("#CheckScheduleEditButton").click(function () {
    var checkInDate = $('#checkin-date').val();
    var checkInTime = $('#checkin-time').val();
    var checkInDateTime = checkInDate && checkInTime ? checkInDate + 'T' + checkInTime : null;
    var checkOutDate = $('#checkout-date').val();
    var checkOutTime = $('#checkout-time').val();
    var checkOutDateTime = checkOutDate && checkOutTime ? checkOutDate + 'T' + checkOutTime : null;

    var attendData = {
        AttendanceId: $('#attendanceid').val(),
        CollaboratorId: $('#collaboratorid').val(),
        CheckIn: checkInDateTime,
        CheckOut: checkOutDateTime,
        CheckInStatus: $('#checkinstatuswork').val(),
        CheckOutStatus: $('#checkoutstatuswork').val(),
        CommentCheckIn: $('#commentcheckin').val(),
        IsOpenCheckIn: $('#isopencheckin').is(':checked')
    };

    $.ajax({
        url: '/Schedule/EditAssistance',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(attendData),
        success: function (response) {
            if (response.success) {
                new Messi(response.message, {
                    title: 'Éxito',
                    titleClass: 'anim success',
                    buttons: [{ id: 0, label: 'Aceptar', val: 'X' }],
                    callback: function () {
                        $("#refreshTable").click();
                    }
                });
            } else {
                new Messi(response.message + ': ' + response.error, {
                    title: 'Error',
                    titleClass: 'anim error',
                    buttons: [{ id: 0, label: 'Cerrar', val: 'X' }]
                });
            }
        },
        error: function (xhr, status, error) {
            new Messi('Error al guardar los datos: ' + error, {
                title: 'Error',
                titleClass: 'anim error',
                buttons: [{ id: 0, label: 'Cerrar', val: 'X' }]
            });
        }
    });
})

$("#refreshTable").click(function () {
    var gridData = $('#jqGrid2').jqGrid('getRowData');
    var collaboratorId = gridData[0]?.CollaboratorId; // Validar que haya datos

    $.ajax({
        url: '/Schedule/GetAssistanceByCollaborator/',
        type: 'GET',
        data: { collaboratorId: collaboratorId },
        dataType: 'json',
        success: function (response) {
            $("#jqGrid2").jqGrid('clearGridData');
            var collaboratorName = 'Sin registros';
            var collaboratorPicture = '/Images/DefaultCollaborator.jpg';

            console.log(response);

            if (response.success && response.data) {
                collaboratorName = response.data.Firstname + ' ' + response.data.Lastname;
                collaboratorPicture = response.data.Picture;

                if (response.data.AttendModels && response.data.AttendModels.length > 0) {
                    response.data.AttendModels.forEach(function (item) {
                        item.IsOpenCheckIn = item.IsOpenCheckIn ? 'Sí' : 'No';
                    });

                    $("#jqGrid2").jqGrid('setGridParam', { data: response.data.AttendModels });
                }
            }

            // Actualizar encabezado de la sección
            $('#SectionHeaderTitle').text(collaboratorName);
            $('#SectionHeaderPicture').attr('src', collaboratorPicture);

            // Ejecutar la recarga del grid y automáticamente activar loadComplete
            $("#jqGrid2").trigger('reloadGrid');
        },
        error: function () {
            alert('Ocurrió un error al intentar obtener los datos del colaborador.');
        }
    });
});

$(document).ready(function () {
    function applyDateFilter() {
        const beginDate = $("#beginDate").val();
        const endDate = $("#endDate").val();

        if (!beginDate && !endDate) {
            var dialog = new Messi('Por favor, seleccione un rango de fechas para filtrar.',
                {
                    title: 'Seleccione fechas',
                    titleClass: 'anim warning',
                    buttons: [{ id: 0, label: 'Cerrar', val: 'X' }]
                }
            );
            return;
        }

        if (!beginDate || !endDate) {
            var dialog = new Messi('Por favor, seleccione ambas fechas',
                {
                    title: 'Seleccione fechas',
                    titleClass: 'anim warning',
                    buttons: [{ id: 0, label: 'Cerrar', val: 'X' }]
                }
            );
            return;
        }

        if (new Date(endDate) < new Date(beginDate)) {
            var dialog = new Messi(
                "La fecha final no puede ser menor a la fecha inicial. \nPor favor, seleccione fechas válidas.",
                {
                    title: "Seleccione fechas",
                    titleClass: "anim warning",
                    buttons: [{ id: 0, label: "Cerrar", val: "X" }]
                }
            );
            return;
        }

       const formatDate = (date) => {
    const [year, month, day] = date.split("-");
    return `${day}-${month}-${year}`;
};

        const formattedBeginDate = formatDate(beginDate);
        const formattedEndDate = formatDate(endDate);

        $("#jqGrid2").jqGrid('setGridParam', {
            postData: {
                filters: JSON.stringify({
                    groupOp: "AND",
                    rules: [
                        { field: "CheckIn", op: "ge", data: formattedBeginDate },
                        { field: "CheckIn", op: "le", data: formattedEndDate }
                    ]
                })
            },
            search: true
        }).trigger("reloadGrid");
    }

    // Botón para filtrar
    $("#filterButton").click(function () {
        applyDateFilter();
    });

    // Botón para recargar la tabla
    $("#refreshTable").click(function () {
        $("#beginDate").val("");
        $("#endDate").val("");
        $("#jqGrid2").jqGrid('setGridParam', {
            postData: {
                filters: null
            },
            search: false
        }).trigger("reloadGrid");
    });
});