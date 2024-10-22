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