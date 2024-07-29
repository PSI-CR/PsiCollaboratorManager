$grid = $("#jqGrid").jqGrid({
    url: "/Annotation/GetAll",
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { label: 'Anotacion Id', name: 'AnnotationId', editable: false, key: true, hidden: true, align: 'center' },
        { label: 'Tipo de anotación', name: 'AnnotationTypeName', editable: false, align: 'center' },
        {
            label: 'Fecha', name: 'Date', formatter: 'date', align: 'center', formatoptions: { newformat: 'd/m/Y' },
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
        { label: 'Nombre', name: 'CollaboratorFirstName',editable: false, align: 'center', headerClasses: 'my-column', cellattr: function () { return 'class="my-column"'; } },
        { label: 'Apellidos', name: 'CollaboratorLastName', align: 'center'},
        { label: 'Cédula', name: 'CollaboratorDNICollaborator', align: 'center' },
        { label: 'Operador', name: 'CollaboratorOperatorNumber', align: 'center' },
        { label: 'E-mail', name: 'CollaboratorEmail', align: 'center', formatter: 'email' }
    ],

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

    ondblClickRow: function (rowId) {
        var rowData = jQuery(this).getRowData(rowId);
        var id = rowData['AnnotationId'];
        getAnnotationsDetails(id, "AnnotationByCollaboratorModal");
    },

    loadonce: true,
    shrinkToFit: true,
    autowidth: true,
    autoresizeOnLoad: true,
    autoresizeOnResize: true,
    height: '100%',
    altRows: true,
    pager: '#jqGridPager',
    rowNum: 20,
    rowList: [10, 20, 30, 50],
    viewrecords: true,
    rownumbers: true,
    sortable: true
});


$(window).on("resize", function () {
    resizeGrid();
});
function resizeGrid() {
    $("#jqGrid").jqGrid("setGridWidth", $("#jqGrid").closest(".ui-jqgrid").parent().width());
}

$('#jqGrid').navGrid('#jqGridPager',
    { edit: false, add: false, del: false, search: true, refresh: true, view: false, position: "left", cloneToTop: true }, { multipleSearch: true, multipleGroup: true }
);

$('#jqGrid').navButtonAdd('#jqGridPager',
    {
        buttonicon: "ui-icon-calculator", title: "Column chooser", caption: "Columnas", position: "last",
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
function resizeJqGridWidth(grid_id, div_id, width) {
    $(window).bind('resize', function () {
        $('#' + grid_id).setGridWidth(width, true);
        $('#' + grid_id).setGridWidth($('#' + div_id).width(), true);
    }).trigger('resize');
}
resizeJqGridWidth("jqGrid", "divTableEmployees", "500");