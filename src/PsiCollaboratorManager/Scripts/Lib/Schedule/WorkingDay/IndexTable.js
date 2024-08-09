$(function () {
    $grid = $("#jqGrid").jqGrid({
        url: '/WorkingDay/GetAllWorkingDay',
        mtype: 'GET',
        datatype: 'json',
        shrinkToFit: false,
        colNames: ['JornadaId', 'Jornada', 'Descripción', 'Asignado', 'Máximo Dias', 'Máximo Horas', 'Hora Inicial', 'Hora Final'],
        colModel: [
            { name: 'WorkingDayId', index: 'JornadaId', width: 36, hidden: true, align: 'center' },
            { name: 'Name', index: 'Jornada', width: 160, editable: true, align: 'center', editrules: { required: true } },
            { name: 'Description', index: 'Descripción', width: 461, editable: true, align: 'center', editrules: { required: true } },
            { name: 'Assigned', index: 'Asignado', width: 166, editable: true, align: 'center', editrules: { required: true } },
            { name: 'MaxDays', index: 'Máximo Dias', width: 166, editable: true, align: 'center', editrules: { required: true } },
            { name: 'MaxHours', index: 'Máximo Horas', width: 166, editable: true, align: 'center', editrules: { required: true } },
            { name: 'StartTime', index: 'Hora Inicial', width: 166, editable: true, align: 'center', editrules: { required: true }, formatter: formatTime },
            { name: 'EndTime', index: 'Hora Final', width: 166, editable: true, align: 'center', editrules: { required: true }, formatter: formatTime },
        ],

        onInitGrid: function () {
            $("<div class='alert-info-grid'>No Record(s) Found</div>").insertAfter($(this).parent());
        },

        loadComplete: function () {
            var $this = $(this), p = $this.jqGrid("getGridParam");
            if (p.reccount === 0) {
                $this.hide();
                $this.parent().siblings(".alert-info-grid").show();
            } else {
                $this.show();
                $this.parent().siblings(".alert-info-grid").hide();
            }
        },

        resizeStop: function (newWidth, index) {
            var colModel = $(this).jqGrid('getGridParam', 'colModel');
            var column = colModel[index];

            console.log(newWidth);
            if (column.width < column.minWidth) {
                $(this).jqGrid('setGridParam', {
                    colModel: colModel
                }).trigger('resize');
            }
        },

        loadonce: true,
        shrinkToFit: true,
        altRows: true,
        pager: '#jqGridPager',
        rowNum: 10,
        rowList: [10, 20, 30, 50],
        viewrecords: true,
        rownumbers: true,
        sortable: true,
        autowidth: true,
        autoresizeOnResize: true,
        height: '100%'
    });

    function formatTime(cellValue, options, rowObject) {
        if (cellValue) {
            var date = new Date(parseInt(cellValue.substr(6)));
            var hours = date.getHours().toString().padStart(2, '0');
            var minutes = date.getMinutes().toString().padStart(2, '0');
            var seconds = date.getSeconds().toString().padStart(2, '0');

            return hours + ':' + minutes + ':' + seconds;
        } else {
            return '';
        }
    }

    $(window).on("resize", function () {
        $("#jqGrid").jqGrid("setGridWidth", $("#jqGrid").closest(".ui-jqgrid").parent().width());
    });

    $('#jqGrid').navGrid('#jqGridPager',
        { edit: false, add: false, del: false, search: false, refresh: false, view: false, position: "left", cloneToTop: true },
        { multipleSearch: true, multipleGroup: true },
    );

    $('#jqGrid').navButtonAdd('#jqGridPager',
        {
            buttonicon: "ui-icon-plusthick",
            title: "Agregar",
            caption: "Agregar",
            position: "last",
            onClickButton: addWorkinDay
        }
    );

    $('#jqGrid').navButtonAdd('#jqGridPager',
        {
            buttonicon: "ui-icon-trash",
            title: "Eliminar",
            caption: "Eliminar",
            position: "last",
            onClickButton: deleteWorkinDay
        }
    );
});

function addWorkinDay() {
    window.location.href = "/WorkingDay/Create";
}

function deleteWorkinDay() {
    var grid = $("#jqGrid");
    var selRowId = grid.jqGrid('getGridParam', 'selrow');
    if (selRowId) {
        var rowData = grid.jqGrid('getRowData', selRowId);
        var workingDayId = rowData.WorkingDayId;

        let formData = new FormData();
        formData.append("workingDayId", workingDayId);
        $.ajax({
            type: "POST",
            url: '../WorkingDay/Delete',
            data: formData,
            dataType: "json",
            cache: false,
            contentType: false,
            processData: false,
            success: function (result) {
                showMessiMessage(result.message, result.success ? 'success' : 'error');
                if (result.success) {                  
                    setTimeout(function () {
                        window.location.href = "../WorkingDay/Index";
                    }, 3000); 
                }
            },
            error: function () {
                showMessiMessage('Hubo un problema al procesar la solicitud.', 'error');
            }
        });
    } else {
        showMessiMessage("Por favor, seleccione una fila para eliminar.", 'warning');
    }
}

function showMessiMessage(message, type) {
    var titleClass, title;

    switch (type) {
        case 'success':
            titleClass = 'anim success';
            title = 'Éxito';
            break;
        case 'error':
            titleClass = 'anim error';
            title = 'Error';
            break;
        case 'warning':
            titleClass = 'anim warning';
            title = 'Advertencia';
            break;
    }
    new Messi(message, { title: title, titleClass: titleClass, buttons: [{ id: 0, label: 'Cerrar', val: 'X' }] });
}