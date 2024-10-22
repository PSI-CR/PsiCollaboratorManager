$grid = $("#jqGrid").jqGrid({
    url: '/Collaborator/GetAllInactive',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { label: 'Id', name: 'CollaboratorId', editable: false, key: true, hidden: true},
        { label: 'Nombre', name: 'FirstName', editable: false, align: 'center' },
        { label: 'Apellidos', name: 'LastName', align: 'center' },
        { label: 'Operador', name: 'OperatorNumber', align: 'center', sorttype: 'number' },
        { label: 'Cédula', name: 'DNICollaborator', align: 'center', sorttype: 'number' },
        { label: 'E-mail', name: 'Email', align: 'center' },
        { label: 'Teléfono', name: 'Telephone1', align: 'center', sorttype: 'number' },
        { label: 'Teléfono 2', name: 'Telephone2', align: 'center', sorttype: 'number', hidden: true },
        { label: 'Nacimiento', name: 'DateOfBirth', formatter: 'date', align: 'center', sorttype: 'date', formatoptions: { newformat: 'd/m/Y' },
            searchoptions: {
                dataInit: function (element) {
                    $(element).datepicker({
                        id: 'orderDate_datePicker',
                        dateFormat: 'dd/mm/yy',
                        maxDate: new Date(2030, 0, 1),
                        showOn: 'focus'
                    });
                }
            }
        },
        { label: 'Género', name: 'Gender', align: 'center', hidden: true },
        { label: "Hijos", name: 'Parent', formatter: "checkbox", formatoptions: { disabled: true }, align: 'center' },
        { label: "Casado", name: 'MaritalStatus', formatter: "checkbox", formatoptions: { disabled: true }, align: 'center' },
        { label: 'Provincia', name: 'ProvinceName', align: 'center', hidden: true },
        { label: 'Cantón', name: 'CantonName', align: 'center', hidden: true },
        { label: 'Distrito', name: 'DistrictName', align: 'center', hidden: true },
        { label: 'Dirección', name: 'AddressLine', align: 'center', hidden: true }
    ],
    onInitGrid: function () {
        $("<div class='alert-info-grid'>No Record(s) Found</div>").insertAfter($(this).parent());
    },
    loadComplete: function () {
        resizeGrid();
        var $this = $(this), p = $this.jqGrid("getGridParam");
        if (p.reccount === 0) {
            $this.hide();
            $this.parent().siblings(".alert-info-grid").show();
        } else {
            $this.show();
            $this.parent().siblings(".alert-info-grid").hide();
        }
    },
    ondblClickRow: function () {
        displayDetails();
    },

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
    autoresizeOnLoad: true,
    autoresizeOnResize: true,
    gridComplete: initGrid,
    height: '100%',
    width: '100%'
});
$(window).on("resize", function () {
    resizeGrid();
});
function resizeGrid() {
    $("#jqGrid").jqGrid("setGridWidth", $("#jqGrid").closest(".ui-jqgrid").parent().width());
}

$('#jqGrid').navGrid('#jqGridPager', { edit: false, add: false, del: false, search: true, refresh: true, view: false, position: "left", cloneToTop: true }, { multipleSearch: true, multipleGroup: true } );

$('#jqGrid').navButtonAdd('#jqGridPager', { buttonicon: "ui-icon-calculator", title: "Column chooser", caption: "Columnas", position: "last",
    onClickButton: function () {
        jQuery("#jqGrid").jqGrid('columnChooser', {
            done: function (numColumn) {
                $("#jqGrid").jqGrid('setGridParam', { }).trigger('resize');
            }
        });
    }
});
$('#jqGrid').navButtonAdd('#jqGridPager', { buttonicon: "ui-icon-pencil", title: "Edit", caption: "Editar", position: "last", onClickButton: editRow });

$('#jqGrid').navButtonAdd('#jqGridPager', { buttonicon: "ui-icon-trash", title: "Delete", caption: "Eliminar", position: "last", onClickButton: deleteRow });
function initGrid() {
    $(".jqgrow", "#jqGrid").contextMenu('contextMenu', {
        bindings: {
            'edit': function (t) { editRow(); },
            'delete': function (t) { deleteRow(); },
            'details': function (t) { displayDetails(); }
        },
        onContextMenu: function (event, menu) {
            var rowId = $(event.target).parent("tr").attr("id")
            var grid = $("#jqGrid");
            grid.setSelection(rowId);
            return true;
        }
    });
}
function editRow() {
    var grid = $("#jqGrid");
    var rowKey = grid.getGridParam("selrow");
    if (rowKey) {

        var key = $("#jqGrid").jqGrid('getCell', rowKey, 'CollaboratorId');
        var editUrl = '/Collaborator/Edit?collaboratorId=' + key;
        window.location.href = editUrl;
    }
    else {
        new Messi("No hay una fila seleccionada",
            {
                title: 'Seleccione un colaborador',
                titleClass: 'anim warning',
                buttons: [{ id: 0, label: 'Cerrar', val: 'X' }]
            }
        );
    }
}
function deleteRow() {
    var grid = $("#jqGrid");
    var rowKey = grid.getGridParam("selrow");
    if (rowKey) {
        var idCollaborator = grid.jqGrid('getCell', rowKey, 'CollaboratorId');
        deleteCollaborator(idCollaborator);
    } else {
        new Messi("No hay una fila seleccionada",
            {
                title: 'Seleccione un colaborador',
                titleClass: 'anim warning',
                buttons: [{ id: 0, label: 'Cerrar', val: 'X' }]
            }
        );
    }
}
function displayDetails() {
    var grid = $("#jqGrid");
    var rowKey = grid.getGridParam("selrow");
    if (rowKey) {
        var id = $("#jqGrid").jqGrid('getCell', rowKey, 'CollaboratorId');
        GetCollaboratorDetails(id, "DetailsModal");
    }
}

function deleteCollaborator(collaboratorId) {
    $.ajax({
        url: "/Collaborator/Delete",
        type: "POST",
        data: { collaboratorId: collaboratorId },
        success: function (result) {
            if (result.success) {
                new Messi("Este colaborador fue eliminado correctamente",
                    {
                        title: 'Success',
                        titleClass: 'anim success',
                        modal: true
                    }).show();
                location.reload();
            } else {
                new Messi("Este colaborador tiene otras dependencias, por favor quitarlas para poder eliminar al colaborador",
                    {
                        title: 'Eliminar dependencias',
                        titleClass: 'anim warning',
                        buttons: [{ id: 0, label: 'Cerrar', val: 'X' }]
                    }
                );               
            }
        },
        error: function (xhr, status, error) {        
            new Messi("Ocurrió un error al eliminar el empleado",
                {
                    title: 'Error al elimiar',
                    titleClass: 'anim error',
                    buttons: [{ id: 0, label: 'Cerrar', val: 'X' }]
                }
            );
        }
    });
}