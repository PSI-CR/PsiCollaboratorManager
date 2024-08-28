
$(document).ready(function () {

    $("#jqGrid2").jqGrid({
        url: '',
        mtype: 'GET',
        datatype: 'local',
        colNames: [
            'AttendId',
            'CheckIn',
            'CheckOut',
            'ValCheckIn',
            'Estado CheckIn',
            'ValCheckOut',
            'Estado CheckOut',
            'Comentario CheckIn',
            'Abierto CheckIn',
            'Tiempo Total'
        ],
        colModel: [
            { name: 'AttendId', index: 'AttendId', width: 80, align: 'center', hidden: true },
            { name: 'CheckIn', index: 'CheckIn', width: 120, align: 'center' },
            { name: 'CheckOut', index: 'CheckOut', width: 120, align: 'center' },
            { name: 'CheckInStatus', index: 'CheckInStatus', width: 100, align: 'center', hidden: true },
            { name: 'CheckInStatusWork', index: 'CheckInStatusWork', width: 100, align: 'center' },
            { name: 'CheckOutStatus', index: 'CheckOutStatus', width: 100, align: 'center', hidden: true },
            { name: 'CheckOutStatusWork', index: 'CheckOutStatusWork', width: 100, align: 'center' },
            { name: 'CommentCheckIn', index: 'CommentCheckIn', width: 200, align: 'center', editable: true },
            { name: 'IsOpenCheckIn', index: 'IsOpenCheckIn', width: 100, align: 'center', editable: true },
            { name: 'TotalTime', index: 'TotalTime', width: 120, align: 'center' } // Nueva columna
        ],
        pager: '#jqGridPager2',
        rowNum: 10,
        rowList: [10, 20, 30],
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
                var checkInDateTime = rowData.CheckIn ? new Date(rowData.CheckIn) : null;
                var checkOutDateTime = rowData.CheckOut ? new Date(rowData.CheckOut) : null;

                if (checkInDateTime && checkOutDateTime && !isNaN(checkInDateTime) && !isNaN(checkOutDateTime)) {
                    var differenceInMilliseconds = checkOutDateTime - checkInDateTime;
                    var differenceInMinutes = Math.floor(differenceInMilliseconds / (1000 * 60));
                    var hours = Math.floor(differenceInMinutes / 60);
                    var minutes = differenceInMinutes % 60;
                    var totalTime = hours + 'h ' + minutes + 'm';

                    grid.jqGrid('setCell', rowId, 'TotalTime', totalTime);
                    totalMinutes += differenceInMinutes; 
                } else {
                   
                    grid.jqGrid('setCell', rowId, 'TotalTime', '0h 0m');
                }
            });

            var totalHours = Math.floor(totalMinutes / 60);
            var remainingMinutes = totalMinutes % 60;
            var totalTimeSum = totalHours + 'h ' + remainingMinutes + 'm';

            $('#totalTimeTextbox').val(totalTimeSum);
        }
    });

    $("#jqGrid2").jqGrid('filterToolbar', { searchOperators: false, searchOnEnter: false, defaultSearch: "cn" });

    $("#jqGrid2").jqGrid('setGridParam', {
        ondblClickRow: function (rowId) {
            var rowData = $("#jqGrid2").jqGrid('getRowData', rowId);

            var checkInDateTime = rowData.CheckIn ? new Date(rowData.CheckIn) : null;
            var checkInDate = checkInDateTime ? checkInDateTime.toISOString().split('T')[0] : '';
            var checkInTime = checkInDateTime ? checkInDateTime.toTimeString().split(' ')[0].substring(0, 5) : '';

            var checkOutDateTime = rowData.CheckOut ? new Date(rowData.CheckOut) : null;
            var checkOutDate = checkOutDateTime ? checkOutDateTime.toISOString().split('T')[0] : '';
            var checkOutTime = checkOutDateTime ? checkOutDateTime.toTimeString().split(' ')[0].substring(0, 5) : '';

            $('#checkin-date').val(checkInDate);
            $('#checkin-time').val(checkInTime);
            $('#checkout-date').val(checkOutDate);
            $('#checkout-time').val(checkOutTime);
            $('#checkinstatuswork').val(rowData.CheckInStatus);
            $('#checkoutstatuswork').val(rowData.CheckOutStatus);
            $('#commentcheckin').val(rowData.CommentCheckIn);
            $('#attendanceid').val(rowData.AttendId);
            $('#isopencheckin').prop('checked', rowData.IsOpenCheckIn === 'Sí');
            OpenModal('EditAssistanceModal');
        }
    });

    $("#jqGrid2").jqGrid('navGrid', '#jqGridPager2', { edit: false, add: false, del: false });

    $("#CheckScheduleEditButton").click(function () {
        var checkInDate = $('#checkin-date').val();
        var checkInTime = $('#checkin-time').val();
        var checkInDateTime = checkInDate && checkInTime ? checkInDate + 'T' + checkInTime + ':00' : null;

        var checkOutDate = $('#checkout-date').val();
        var checkOutTime = $('#checkout-time').val();
        var checkOutDateTime = checkOutDate && checkOutTime ? checkOutDate + 'T' + checkOutTime + ':00' : null;

        var attendData = {
            AttendanceId: $('#attendanceid').val(),
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
                            location.reload();
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
});
