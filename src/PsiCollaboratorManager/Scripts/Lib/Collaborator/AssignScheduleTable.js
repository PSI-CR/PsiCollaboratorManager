$grid = $("#jqGrid").jqGrid({
    url: '/Collaborator/GetCollaboratorsWithSchedule',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { label: 'Id', name: 'CollaboratorId', editable: false, key: true, hidden: true, align: 'center' },
        { label: 'Nombre', name: 'FirstName', align: 'center', headerClasses: 'my-column', cellattr: function () { return 'class="my-column"'; } },
        { label: 'Apellidos', name: 'LastName', align: 'center' },
        { label: 'Cedula', name: 'DNICollaborator', align: 'center' },
        { label: 'Operador', name: 'OperatorNumber', align: 'center' },
        { label: 'E-mail', name: 'Email', align: 'center', formatter: 'email' },
        { label: 'Fecha de Asignacion', name: 'AssignDate', align: 'center', formatter: 'date', formatoptions: { newformat: 'd/m/Y' }, searchoptions: { sopt: ['eq'] } },
        { label: 'Horario', name: 'ScheduleName', align: 'center' }
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
        resizeGrid();
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
    //ondblClickRow: function (rowId) {
    //    var rowData = jQuery(this).getRowData(rowId);
    //    var id = rowData['CollaboratorId'];
    //    getEmployeeDetails(id);
    //},
    loadonce: true,
    altRows: true,
    pager: '#jqGridPager',
    rowNum: 20,
    rowList: [10, 20, 30, 50],
    viewrecords: true,
    rownumbers: true,
    sortable: true,
    shrinkToFit: true,
    autowidth: true,
    autoresizeOnLoad: true,
    autoresizeOnResize: true,
    height: '100%',
    gridComplete: initGrid
});

$(window).on("resize", function () {
    resizeGrid();
});
function resizeGrid() {
    $("#jqGrid").jqGrid("setGridWidth", $("#jqGrid").closest(".ui-jqgrid").parent().width());
}

$('#jqGrid').navGrid('#jqGridPager',
    { edit: false, add: false, del: false, search: true, refresh: true, view: false, position: "left", cloneToTop: true }, { multipleSearch: true, multipleGroup: true });


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
});

$('#jqGrid').navButtonAdd('#jqGridPager',
{
    buttonicon: "ui-icon-pencil",
    title: "Dismiss",
    caption: "Quitar Horario",
    position: "last",
    onClickButton: DismissSchedule
});
function resizeJqGridWidth(grid_id, div_id, width) {
    $(window).bind('resize', function () {
        $('#' + grid_id).setGridWidth(width, true);
        $('#' + grid_id).setGridWidth($('#' + div_id).width(), true);
    }).trigger('resize');
}

function initGrid() {
    $(".jqgrow", "#jqGrid").contextMenu('contextMenu', {
        bindings: {
            'dismiss': function (t) {
                DismissSchedule();
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