$grid = $("#jqGrid").jqGrid({
    url: '/Collaborator/GetAllActive',
    mtype: 'GET',
    datatype: 'json',
    shrinkToFit: false,
    colModel: [
        { label: 'Id', name: 'CollaboratorId', editable: false, key: true, hidden: true},
        { label: 'Nombre', name: 'FirstName', editable: false, align: 'center' },
        { label: 'Apellidos', name: 'LastName', align: 'center' },
        { label: 'Operador', name: 'OperatorNumber', align: 'center' },
        { label: 'Cédula', name: 'DNICollaborator', align: 'center' },
        { label: 'E-mail', name: 'Email', align: 'center' },
        { label: 'Teléfono', name: 'Telephone1', align: 'center' },
        { label: 'Teléfono 2', name: 'Telephone2', align: 'center', hidden: true },
        {
            label: 'Nacimiento', name: 'DateOfBirth', formatter: 'date', align: 'center', formatoptions: { newformat: 'd/m/Y' },
            searchoptions: {
                dataInit: function (element) {
                    $(element).datepicker({
                        id: 'orderDate_datePicker',
                        dateFormat: 'yy-mm-dd',
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
        if (column.width < column.minWidth) {
            $(this).jqGrid('setGridParam', {
                colModel: colModel
            }).trigger('resize');
        }
    },

    ondblClickRow: function (rowId) {
        var rowData = jQuery(this).getRowData(rowId);
        var id = rowData['CollaboratorId'];
        GetCollaboratorDetails(id, "DetailsModal");
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
    $("#jqGrid").jqGrid("setGridWidth", $("#jqGrid").closest(".ui-jqgrid").parent().width());
});

$('#jqGrid').navGrid('#jqGridPager',
    { edit: false, add: false, del: false, search: true, refresh: true, view: false, position: "left", cloneToTop: true }, { multipleSearch: true, multipleGroup: true }
);

$('#jqGrid').navButtonAdd('#jqGridPager',
    {
        buttonicon: "ui-icon-calculator",
        title: "Column chooser",
        caption: "Columnas",
        position: "last",
        onClickButton: function () {
            jQuery("#jqGrid").jqGrid('columnChooser', {
                done: function (numColumn) {
                    $("#jqGrid").jqGrid('setGridParam', {
                    }).trigger('resize');
                }
            });
        }
    }
);
$('#jqGrid').navButtonAdd('#jqGridPager',
    {
        buttonicon: "ui-icon-pencil",
        title: "Edit",
        caption: "Editar",
        position: "last",
        onClickButton: editRow
    }
);
$('#jqGrid').navButtonAdd('#jqGridPager',
    {
        buttonicon: "ui-icon-plusthick",
        title: "Add",
        caption: "Agregar",
        position: "last",
        onClickButton: addRow
    }
);
function resizeJqGridWidth(grid_id, div_id, width) {
    $(window).bind('resize', function () {
        $('#' + grid_id).setGridWidth(width, true);
        $('#' + grid_id).setGridWidth($('#' + div_id).width(), true);
    }).trigger('resize');
}

function initGrid() {
    $(".jqgrow", "#jqGrid").contextMenu('contextMenu', {
        bindings: {
            'edit': function (t) {
                editRow();
            },
            'add': function (t) {
                addRow();
            }
            ,
            'details': function (t) {
                displayDetails();
            }
        },
        onContextMenu: function (event, menu) {
            var rowId = $(event.target).parent("tr").attr("id")
            var grid = $("#jqGrid");
            grid.setSelection(rowId);
            return true;
        }
    });
}

function addRow() {
    var editUrl = '/Collaborator/Create';
    window.location.href = editUrl;
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
        new Messi('Favor seleccione un colaborador para editar', {
            title: 'Seleccione Colaborador',
            titleClass: 'anim warning',
            modal: true,
            styles: {
                width: '500px',
                marginLeft: '170px',
                marginTop: '-164px'
            }
        });
    }
}