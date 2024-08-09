$grid = $("#jqGrid").jqGrid({
    url: '/Schedule/GetCollaborator',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { label: 'Id', name: 'CollaboratorId', key: true, hidden: true },
        { label: 'Nombre', name: 'FirstName', align: 'center' },
        { label: 'Apellidos', name: 'LastName', align: 'center' },
        { label: 'Número Operador', name: 'OperatorNumber', align: 'center' }
    ],
    loadonce: true,
    shrinkToFit: true,
    altRows: true,
    pager: '#jqGridPager',
    rowNum: 30,
    rowList: [10, 20, 30, 50],
    viewrecords: true,
    rownumbers: true,
    sortable: true,
    autowidth: true,
    height: '100%',
    width: '100%',

    onInitGrid: function () {
        $("<div class='alert-info-grid'>Sin registros</div>").insertAfter($(this).parent());
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

    ondblClickRow: function (rowId) {
        var rowData = jQuery(this).getRowData(rowId);
        var collaboratorId = rowData['CollaboratorId'];
        window.location.href = '/Schedule/GetAssistanceByCollaborator/?colaboratorId=' + collaboratorId;
    },

    error: function (jqXHR, textStatus, errorThrown) {
        alert("Error al cargar los datos: " + textStatus + " - " + errorThrown);
    }
});

$(document).ready(function () {
  
    var $grid = $("#jqGrid2").jqGrid({
        url: '',
        mtype: 'GET',
        datatype: 'json',
        colNames: ['Id', 'Colaborador Id', 'CheckIn', 'CheckOut', 'Estado CheckIn', 'Estado CheckOut', 'Comentario CheckIn', 'Abierto CheckIn', 'Dirección IP', 'Dirección Física'],
        colModel: [
            { name: 'AttendId', index: 'AttendId', width: 75, align: 'center', key: true },
            { name: 'CollaboratorId', index: 'CollaboratorId', width: 100, align: 'center' },
            { name: 'CheckIn', index: 'CheckIn', width: 150, align: 'center' },
            { name: 'CheckOut', index: 'CheckOut', width: 150, align: 'center' },
            { name: 'CheckInStatus', index: 'CheckInStatus', width: 100, align: 'center' },
            { name: 'CheckOutStatus', index: 'CheckOutStatus', width: 100, align: 'center' },
            { name: 'CommentCheckIn', index: 'CommentCheckIn', width: 200, align: 'center' },
            { name: 'IsOpenCheckIn', index: 'IsOpenCheckIn', width: 100, align: 'center' },
            { name: 'IpAddress', index: 'IpAddress', width: 150, align: 'center' },
            { name: 'PhysicalAddressEquipment', index: 'PhysicalAddressEquipment', width: 200, align: 'center' }
        ],
        pager: '#jqGridPager2',
        rowNum: 10,
        rowList: [10, 20, 30],
        sortname: 'AttendId',
        sortorder: 'asc',
        viewrecords: true,
        gridview: true,
        autoencode: true,
        height: 'auto',
        width: '100%',
        caption: "Asistencia de Colaboradores",
        loadonce: true
    });

    // Configura el paginador para la segunda tabla (jqGrid2)
    $grid.jqGrid('navGrid', '#jqGridPager2', { edit: false, add: false, del: false });

    // Inicializa la primera tabla (jqGrid) y define el evento ondblClickRow
    $("#jqGrid").jqGrid({
        ondblClickRow: function (rowId) {
            var rowData = jQuery(this).getRowData(rowId);
            var collaboratorId = rowData['CollaboratorId'];

            // Actualiza la URL de la tabla jqGrid2 con el colaboradorId
            var newUrl = '/Schedule/GetAssistanceByCollaborator/?colaboratorId=' + collaboratorId;
            $grid.setGridParam({ url: newUrl, datatype: 'json' }).trigger('reloadGrid');
        }
    });
});