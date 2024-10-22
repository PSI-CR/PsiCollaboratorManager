$(document).ready(function () {
    $("#jqGrid2").jqGrid({
        url: '',
        mtype: 'GET',
        datatype: 'local',
        colNames: [
            'AttendId',
            'CollaboratorId',
            'CheckIn',
            'CheckOut',
            'ValCheckIn',
            'Estado CheckIn',
            'ValCheckOut',
            'Estado CheckOut',
            'Comentario CheckIn',
            'Salida',
            'Tiempo Total'
        ],
        colModel: [
            { name: 'AttendId', index: 'AttendId', width: 80, align: 'center', hidden: true },
            { name: 'CollaboratorId', index: 'CollaboratorId', width: 80, align: 'center', hidden: true },
            { name: 'CheckIn', index: 'CheckIn', width: 190, align: 'center' },
            { name: 'CheckOut', index: 'CheckOut', width: 190, align: 'center' },
            { name: 'CheckInStatus', index: 'CheckInStatus', width: 100, align: 'center', hidden: true },
            { name: 'CheckInStatusWork', index: 'CheckInStatusWork', width: 100, align: 'center' },
            { name: 'CheckOutStatus', index: 'CheckOutStatus', width: 100, align: 'center', hidden: true },
            { name: 'CheckOutStatusWork', index: 'CheckOutStatusWork', width: 100, align: 'center' },
            { name: 'CommentCheckIn', index: 'CommentCheckIn', width: 200, align: 'center', editable: true },
            { name: 'IsOpenCheckIn', index: 'IsOpenCheckIn', width: 100, align: 'center', editable: true },
            { name: 'TotalTime', index: 'TotalTime', width: 120, align: 'center' }
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
        toolbar: [true, "top"],
        loadComplete: function () {
            var grid = $(this);
            var rows = grid.jqGrid('getDataIDs');
            var totalMinutes = 0;

            rows.forEach(function (rowId) {
                var rowData = grid.jqGrid('getRowData', rowId);

                // Obtener CheckIn y CheckOut
                var checkIn = rowData.CheckIn;
                var checkOut = rowData.CheckOut;

                // Función para convertir una fecha en formato 'dd/MM/yyyy HH:mm:ss' a un objeto Date
                function parseDateTime(dateTimeStr) {
                    var dateParts = dateTimeStr.split(' ')[0].split('/');
                    var timeParts = dateTimeStr.split(' ')[1].split(':');
                    return new Date(
                        dateParts[2],
                        dateParts[1] - 1,
                        dateParts[0],
                        timeParts[0],
                        timeParts[1],
                        timeParts[2]
                    );
                }

                // Validar y convertir los formatos de CheckIn y CheckOut
                var checkInDateTime = checkIn ? parseDateTime(checkIn) : null;
                var checkOutDateTime = checkOut ? parseDateTime(checkOut) : null;

                if (checkInDateTime && checkOutDateTime && !isNaN(checkInDateTime) && !isNaN(checkOutDateTime)) {
                    // Calcular la diferencia entre check-in y check-out
                    var differenceInMilliseconds = checkOutDateTime - checkInDateTime;
                    var differenceInMinutes = Math.floor(differenceInMilliseconds / (1000 * 60));
                    var hours = Math.floor(differenceInMinutes / 60);
                    var minutes = differenceInMinutes % 60;
                    var totalTime = hours + 'h ' + minutes + 'm';

                    // Establecer el valor de la celda TotalTime
                    grid.jqGrid('setCell', rowId, 'TotalTime', totalTime);
                    totalMinutes += differenceInMinutes;
                } else {
                    // Si no hay CheckIn o CheckOut, establece '0h 0m'
                    grid.jqGrid('setCell', rowId, 'TotalTime', '0h 0m');
                }
            });

            // Cálculo del total de horas
            var totalHours = Math.floor(totalMinutes / 60);
            var remainingMinutes = totalMinutes % 60;
            var totalTimeSum = totalHours + 'h ' + remainingMinutes + 'm';
            $('#totalTimeTextbox').val(totalTimeSum);
        }
    });

    $("#filterButton").click(function () {
        var gridData = $('#jqGrid2').jqGrid('getRowData');
        if (gridData.length === 0) {
            new Messi('Por favor seleccione los datos de un colaborador, para filtrar los datos.', {
                title: 'Error',
                titleClass: 'anim error',
                buttons: [{ id: 0, label: 'Cerrar', val: 'X' }]
            });
        }

        var collaboratorId = gridData[0].CollaboratorId;
        var beginDate = $('#beginDate').val();
        var endDate = $('#endDate').val();

        if (!beginDate || !endDate) {
            new Messi('Por favor ingresa ambas fechas.', {
                title: 'Selección de fechas',
                titleClass: 'anim warning',
                buttons: [{ id: 0, label: 'Cerrar', val: 'X' }]
            });
        }

        var startDateObj = new Date(beginDate);
        var endDateObj = new Date(endDate);

        if (endDateObj < startDateObj) {
            new Messi('La fecha final no puede ser anterior a la fecha inicial.', {
                title: 'Error en las fechas',
                titleClass: 'anim error',
                buttons: [{ id: 0, label: 'Cerrar', val: 'X' }]
            });
        }
        $.ajax({
            url: '/Schedule/GetInformationAttendDatesRangeByCollaborator',
            type: 'GET',
            data: {
                CollaboratorId: collaboratorId,
                BeginTime: beginDate,
                EndTime: endDate
            },
            success: function (response) {
                console.log(response);
                if (response.success) {
                    $('#jqGrid2').jqGrid('clearGridData');
                    var formattedRows = response.rows.map(function (row) {
                        return {
                            ...row,
                            CheckIn: formatDate(row.CheckIn),
                            CheckOut: formatDate(row.CheckOut),
                            IsOpenCheckIn: row.IsOpenCheckIn ? 'Sí' : 'No',
                            CheckInStatusWork: row.LabelCheckInStatus,
                            CheckOutStatusWork: row.LabelCheckOutStatus
                        };
                    });

                    // Cargar los nuevos datos
                    $('#jqGrid2').jqGrid('setGridParam', {
                        datatype: 'jsonstring',
                        datastr: formattedRows
                    }).trigger('reloadGrid');
                } else {
                    new Messi('Error al filtrar los datos: ' + response.error, { title: 'Error' });
                }
            },
            error: function (xhr, status, error) {
                new Messi('Error al realizar la consulta: ' + error, { title: 'Error' });
            }
        });
    });

    function formatDate(dateString) {
        var date = new Date(parseInt(dateString.substr(6))); // Convierte el valor de '/Date(...)' a un objeto Date
        var day = ('0' + date.getDate()).slice(-2); // Asegura dos dígitos para el día
        var month = ('0' + (date.getMonth() + 1)).slice(-2); // Asegura dos dígitos para el mes
        var year = date.getFullYear();
        var hours = ('0' + date.getHours()).slice(-2); // Asegura dos dígitos para las horas
        var minutes = ('0' + date.getMinutes()).slice(-2); // Asegura dos dígitos para los minutos
        var seconds = ('0' + date.getSeconds()).slice(-2); // Asegura dos dígitos para los segundos

        /* return ${ day } /${month}/${ year } ${ hours }:${ minutes }:${ seconds };*/
        return day + '/' + month + '/' + year + ' ' + hours + ':' + minutes /*+ ':' + seconds*/;
    }

    $("#jqGrid2").jqGrid('filterToolbar', { searchOperators: false, searchOnEnter: false, defaultSearch: "cn" });
    $("#jqGrid2").jqGrid('setGridParam', {

        ondblClickRow: function (rowId) {
            var rowData = $("#jqGrid2").jqGrid('getRowData', rowId);

            var checkInDateTime = rowData.CheckIn;
            var checkInDate = checkInDateTime.split(' ')[0];

            // Convertir a formato 'YYYY-MM-DD'
            var dateParts = checkInDate.split('/');
            var formattedDate = dateParts[2] + '-' + dateParts[1] + '-' + dateParts[0];

            // Obtiene el valor 
            var checkInTime = checkInDateTime.split(' ')[1];

            var checkOutDateTime = rowData.CheckOut;
            var checkOutDate = checkOutDateTime.split(' ')[0];

            // Convertir a formato 'YYYY-MM-DD'
            var dateOutParts = checkOutDate.split('/');
            var formattedDateOut = dateOutParts[2] + '-' + dateOutParts[1] + '-' + dateOutParts[0];

            // Obtiene el valor 
            var checkOutTime = checkOutDateTime.split(' ')[1];

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
            OpenModal('EditAssistanceModal');
        }
    });

    $("#jqGrid2").jqGrid('navGrid', '#jqGridPager2', { edit: false, add: false, del: false });
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
                            $("#refreshButton").click();
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
    });

    $("#refreshButton").click(function () {
        var gridData = $('#jqGrid2').jqGrid('getRowData');
        var collaboratorId = gridData[0].CollaboratorId;

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
                        $("#jqGrid2").trigger('reloadGrid');
                    }
                }
                $('#SectionHeaderTitle').text(collaboratorName);
                $('#SectionHeaderPicture').attr('src', collaboratorPicture);
            },
            error: function () {
                alert('Ocurrió un error al intentar obtener los datos del colaborador.');
            }
        });
    });

});

//$(document).ready(function () {

//    $("#jqGrid2").jqGrid({
//        url: '',  // No necesitas url porque lo llenas dinámicamente
//        mtype: 'GET',
//        datatype: 'local',
//        colNames: [
//            'AttendId',
//            'CollaboratorId',
//            'CheckIn',
//            'CheckOut',
//            'ValCheckIn',
//            'Estado CheckIn',
//            'ValCheckOut',
//            'Estado CheckOut',
//            'Comentario CheckIn',
//            'Salida',
//            'Tiempo Total',
//            'Horas Extra',   // Nueva columna para horas extra
//            'Tiempo Faltante' // Nueva columna para tiempo faltante
//        ],
//        colModel: [
//            { name: 'AttendId', index: 'AttendId', width: 80, align: 'center', hidden: true },
//            { name: 'CollaboratorId', index: 'CollaboratorId', width: 80, align: 'center', hidden: true },
//            { name: 'CheckIn', index: 'CheckIn', width: 190, align: 'center' },
//            { name: 'CheckOut', index: 'CheckOut', width: 190, align: 'center' },
//            { name: 'CheckInStatus', index: 'CheckInStatus', width: 100, align: 'center', hidden: true },
//            { name: 'CheckInStatusWork', index: 'CheckInStatusWork', width: 100, align: 'center' },
//            { name: 'CheckOutStatus', index: 'CheckOutStatus', width: 100, align: 'center', hidden: true },
//            { name: 'CheckOutStatusWork', index: 'CheckOutStatusWork', width: 100, align: 'center' },
//            { name: 'CommentCheckIn', index: 'CommentCheckIn', width: 200, align: 'center', editable: true },
//            { name: 'IsOpenCheckIn', index: 'IsOpenCheckIn', width: 100, align: 'center', editable: true },
//            { name: 'TotalTime', index: 'TotalTime', width: 120, align: 'center' },
//            { name: 'ExtraTime', index: 'ExtraTime', width: 120, align: 'center' },  // Columna para horas extra
//            { name: 'MissingTime', index: 'MissingTime', width: 120, align: 'center' } // Columna para tiempo faltante
//        ],
//        pager: '#jqGridPager2',
//        rowNum: 30,
//        rowList: [30, 40, 50],
//        sortorder: 'asc',
//        viewrecords: true,
//        gridview: true,
//        autoencode: true,
//        autowidth: true,
//        height: 'auto',
//        width: '100%',
//        loadonce: true,
//        toolbar: [true, "top"],
//        loadComplete: function () {
//            var grid = $(this);
//            var rows = grid.jqGrid('getDataIDs');
//            var totalMinutes = 0;

//            rows.forEach(function (rowId) {
//                var rowData = grid.jqGrid('getRowData', rowId);

//                // Obtener CheckIn y CheckOut
//                var checkIn = rowData.CheckIn;
//                var checkOut = rowData.CheckOut;

//                // Función para convertir una fecha en formato 'dd/MM/yyyy HH:mm:ss' a un objeto Date
//                function parseDateTime(dateTimeStr) {
//                    var dateParts = dateTimeStr.split(' ')[0].split('/');
//                    var timeParts = dateTimeStr.split(' ')[1].split(':');
//                    return new Date(
//                        dateParts[2],
//                        dateParts[1] - 1,
//                        dateParts[0],
//                        timeParts[0],
//                        timeParts[1],
//                        timeParts[2]
//                    );
//                }

//                // Validar y convertir los formatos de CheckIn y CheckOut
//                var checkInDateTime = checkIn ? parseDateTime(checkIn) : null;
//                var checkOutDateTime = checkOut ? parseDateTime(checkOut) : null;

//                if (checkInDateTime && checkOutDateTime && !isNaN(checkInDateTime) && !isNaN(checkOutDateTime)) {
//                    // Calcular la diferencia entre check-in y check-out
//                    var differenceInMilliseconds = checkOutDateTime - checkInDateTime;
//                    var differenceInMinutes = Math.floor(differenceInMilliseconds / (1000 * 60));
//                    var hours = Math.floor(differenceInMinutes / 60);
//                    var minutes = differenceInMinutes % 60;
//                    var totalTime = hours + 'h ' + minutes + 'm';

//                    // Establecer el valor de la celda TotalTime
//                    grid.jqGrid('setCell', rowId, 'TotalTime', totalTime);
//                    totalMinutes += differenceInMinutes;

//                    // Asegúrate de que las variables sean números
//                    var differenceInMinutes = Number(differenceInMinutes);
//                    var standardWorkMinutes = Number(standardWorkMinutes);

//                    // Verifica si ambos valores son números y positivos
//                    if (!isNaN(differenceInMinutes) && !isNaN(standardWorkMinutes)) {
//                        // Calcula las horas extra solo si el tiempo trabajado excede la jornada laboral estándar
//                        var extraMinutes = Math.max(0, differenceInMinutes - standardWorkMinutes);
//                        console.log('Horas extra (en minutos): ', extraMinutes);
//                    } else {
//                        console.error('Error: Los valores de differenceInMinutes o standardWorkMinutes no son válidos.');
//                    }

//                    var extraTime = Math.floor(extraMinutes / 60) + 'h ' + (extraMinutes % 60) + 'm';
//                    var missingTime = Math.floor(missingMinutes / 60) + 'h ' + (missingMinutes % 60) + 'm';

//                    // Establecer valores para ExtraTime y MissingTime
//                    grid.jqGrid('setCell', rowId, 'ExtraTime', extraTime);
//                    grid.jqGrid('setCell', rowId, 'MissingTime', missingTime);
//                } else {
//                    // Si no hay CheckIn o CheckOut, establece '0h 0m'
//                    grid.jqGrid('setCell', rowId, 'TotalTime', '0h 0m');
//                    grid.jqGrid('setCell', rowId, 'ExtraTime', '0h 0m');
//                    grid.jqGrid('setCell', rowId, 'MissingTime', '0h 0m');
//                }
//            });

//            // Cálculo del total de horas
//            var totalHours = Math.floor(totalMinutes / 60);
//            var remainingMinutes = totalMinutes % 60;
//            var totalTimeSum = totalHours + 'h ' + remainingMinutes + 'm';
//            $('#totalTimeTextbox').val(totalTimeSum);
//        }
//    });

//    $("#filterButton").click(function () {
//        var gridData = $('#jqGrid2').jqGrid('getRowData');
//        if (gridData.length === 0) {
//            new Messi('Por favor seleccione los datos de un colaborador, para filtrar los datos.', {
//                title: 'Error',
//                titleClass: 'anim error',
//                buttons: [{ id: 0, label: 'Cerrar', val: 'X' }]
//            });
//        }

//        var collaboratorId = gridData[0].CollaboratorId;
//        var beginDate = $('#beginDate').val();
//        var endDate = $('#endDate').val();

//        if (!beginDate || !endDate) {
//            new Messi('Por favor ingresa ambas fechas.', {
//                title: 'Selección de fechas',
//                titleClass: 'anim warning',
//                buttons: [{ id: 0, label: 'Cerrar', val: 'X' }]
//            });
//        }

//        var startDateObj = new Date(beginDate);
//        var endDateObj = new Date(endDate);

//        if (endDateObj < startDateObj) {
//            new Messi('La fecha final no puede ser anterior a la fecha inicial.', {
//                title: 'Error en las fechas',
//                titleClass: 'anim error',
//                buttons: [{ id: 0, label: 'Cerrar', val: 'X' }]
//            });
//        }
//        $.ajax({
//            url: '/Schedule/GetInformationAttendDatesRangeByCollaborator',
//            type: 'GET',
//            data: {
//                CollaboratorId: collaboratorId,
//                BeginTime: beginDate,
//                EndTime: endDate
//            },
//            success: function (response) {
//                console.log(response);
//                if (response.success) {
//                    $('#jqGrid2').jqGrid('clearGridData');
//                    var formattedRows = response.rows.map(function (row) {
//                        return {
//                            ...row,
//                            CheckIn: formatDate(row.CheckIn),
//                            CheckOut: formatDate(row.CheckOut),
//                            IsOpenCheckIn: row.IsOpenCheckIn ? 'Sí' : 'No',
//                            CheckInStatusWork: row.LabelCheckInStatus,
//                            CheckOutStatusWork: row.LabelCheckOutStatus
//                        };
//                    });

//                    // Cargar los nuevos datos
//                    $('#jqGrid2').jqGrid('setGridParam', {
//                        datatype: 'jsonstring',
//                        datastr: formattedRows
//                    }).trigger('reloadGrid');
//                } else {
//                    new Messi('Error al filtrar los datos: ' + response.error, { title: 'Error' });
//                }
//            },
//            error: function (xhr, status, error) {
//                new Messi('Error al realizar la consulta: ' + error, { title: 'Error' });
//            }
//        });
//    });

//    // Función para formatear las fechas
//    function formatDate(dateString) {
//        var date = new Date(parseInt(dateString.substr(6))); // Convierte el valor de '/Date(...)' a un objeto Date
//        var day = ('0' + date.getDate()).slice(-2); // Asegura dos dígitos para el día
//        var month = ('0' + (date.getMonth() + 1)).slice(-2); // Asegura dos dígitos para el mes
//        var year = date.getFullYear();
//        var hours = ('0' + date.getHours()).slice(-2); // Asegura dos dígitos para las horas
//        var minutes = ('0' + date.getMinutes()).slice(-2); // Asegura dos dígitos para los minutos
//        var seconds = ('0' + date.getSeconds()).slice(-2); // Asegura dos dígitos para los segundos

//        return ${ day } /${month}/${ year } ${ hours }:${ minutes }:${ seconds };
//    }

//    $("#jqGrid2").jqGrid('filterToolbar', { searchOperators: false, searchOnEnter: false, defaultSearch: "cn" });
//    $("#jqGrid2").jqGrid('setGridParam', {

//        ondblClickRow: function (rowId) {
//            var rowData = $("#jqGrid2").jqGrid('getRowData', rowId);

//            var checkInDateTime = rowData.CheckIn;
//            var checkInDate = checkInDateTime.split(' ')[0];

//            // Convertir a formato 'YYYY-MM-DD'
//            var dateParts = checkInDate.split('/');
//            var formattedDate = dateParts[2] + '-' + dateParts[1] + '-' + dateParts[0];

//            // Obtiene el valor 
//            var checkInTime = checkInDateTime.split(' ')[1];

//            var checkOutDateTime = rowData.CheckOut;
//            var checkOutDate = checkOutDateTime.split(' ')[0];

//            // Convertir a formato 'YYYY-MM-DD'
//            var dateOutParts = checkOutDate.split('/');
//            var formattedDateOut = dateOutParts[2] + '-' + dateOutParts[1] + '-' + dateOutParts[0];

//            // Obtiene el valor 
//            var checkOutTime = checkOutDateTime.split(' ')[1];

//            $('#checkin-date').val(formattedDate);
//            $('#checkin-time').val(checkInTime);
//            $('#checkout-date').val(formattedDateOut);
//            $('#checkout-time').val(checkOutTime);
//            $('#checkinstatuswork').val(rowData.CheckInStatus);
//            $('#checkoutstatuswork').val(rowData.CheckOutStatus);
//            $('#commentcheckin').val(rowData.CommentCheckIn);
//            $('#attendanceid').val(rowData.AttendId);
//            $('#collaboratorid').val(rowData.CollaboratorId);
//            $('#isopencheckin').prop('checked', rowData.IsOpenCheckIn === 'Sí');
//            OpenModal('EditAssistanceModal');
//        }
//    });

//    $("#jqGrid2").jqGrid('navGrid', '#jqGridPager2', { edit: false, add: false, del: false });
//    $("#CheckScheduleEditButton").click(function () {
//        var checkInDate = $('#checkin-date').val();
//        var checkInTime = $('#checkin-time').val();
//        var checkInDateTime = checkInDate && checkInTime ? checkInDate + 'T' + checkInTime : null;

//        var checkOutDate = $('#checkout-date').val();
//        var checkOutTime = $('#checkout-time').val();
//        var checkOutDateTime = checkOutDate && checkOutTime ? checkOutDate + 'T' + checkOutTime : null;

//        var attendData = {
//            AttendanceId: $('#attendanceid').val(),
//            CollaboratorId: $('#collaboratorid').val(),
//            CheckIn: checkInDateTime,
//            CheckOut: checkOutDateTime,
//            CheckInStatus: $('#checkinstatuswork').val(),
//            CheckOutStatus: $('#checkoutstatuswork').val(),
//            CommentCheckIn: $('#commentcheckin').val(),
//            IsOpenCheckIn: $('#isopencheckin').is(':checked')
//        };

//        $.ajax({
//            url: '/Schedule/EditAssistance',
//            type: 'POST',
//            contentType: 'application/json',
//            data: JSON.stringify(attendData),
//            success: function (response) {
//                if (response.success) {
//                    new Messi(response.message, {
//                        title: 'Éxito',
//                        titleClass: 'anim success',
//                        buttons: [{ id: 0, label: 'Aceptar', val: 'X' }],
//                        callback: function () {
//                            $("#refreshButton").click();
//                        }
//                    });
//                } else {
//                    new Messi(response.message + ': ' + response.error, {
//                        title: 'Error',
//                        titleClass: 'anim error',
//                        buttons: [{ id: 0, label: 'Cerrar', val: 'X' }]
//                    });
//                }
//            },
//            error: function (xhr, status, error) {
//                new Messi('Error al guardar los datos: ' + error, {
//                    title: 'Error',
//                    titleClass: 'anim error',
//                    buttons: [{ id: 0, label: 'Cerrar', val: 'X' }]
//                });
//            }
//        });
//    });

//    $("#refreshButton").click(function () {
//        var gridData = $('#jqGrid2').jqGrid('getRowData');
//        var collaboratorId = gridData[0].CollaboratorId;

//        $.ajax({
//            url: '/Schedule/GetAssistanceByCollaborator/',
//            type: 'GET',
//            data: { collaboratorId: collaboratorId },
//            dataType: 'json',
//            success: function (response) {
//                $("#jqGrid2").jqGrid('clearGridData');
//                var collaboratorName = 'Sin registros';
//                var collaboratorPicture = '/Images/DefaultCollaborator.jpg';

//                console.log(response);

//                if (response.success && response.data) {
//                    collaboratorName = response.data.Firstname + ' ' + response.data.Lastname;
//                    collaboratorPicture = response.data.Picture;

//                    if (response.data.AttendModels && response.data.AttendModels.length > 0) {
//                        response.data.AttendModels.forEach(function (item) {
//                            item.IsOpenCheckIn = item.IsOpenCheckIn ? 'Sí' : 'No';
//                        });

//                        $("#jqGrid2").jqGrid('setGridParam', { data: response.data.AttendModels });
//                        $("#jqGrid2").trigger('reloadGrid');
//                    }
//                }
//                $('#SectionHeaderTitle').text(collaboratorName);
//                $('#SectionHeaderPicture').attr('src', collaboratorPicture);
//            },
//            error: function () {
//                alert('Ocurrió un error al intentar obtener los datos del colaborador.');
//            }
//        });
//    });
//});