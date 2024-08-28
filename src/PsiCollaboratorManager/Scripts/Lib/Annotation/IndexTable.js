
$grid = $("#jqGrid").jqGrid({
    url: "/Annotation/GetAll",
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { label: 'Anotacion Id', name: 'AnnotationId', editable: false, key: true, hidden: true, align: 'center' },
        { label: 'Tipo de anotación', name: 'AnnotationTypeName', editable: false, align: 'center' },
        {
            label: 'Fecha', name: 'Date', formatter: 'date', align: 'center', sorttype: 'date', formatoptions: { newformat: 'd/m/Y' },
            searchoptions: {
                sopt: ['eq', 'ne', 'lt', 'le', 'gt', 'ge'], 
                dataInit: function (element) {
                    $(element).datepicker({
                        id: 'orderDate_datePicker',
                        dateFormat: 'dd/mm/yy',
                        maxDate: new Date(2030, 0, 1),
                        showOn: 'focus',
                        onSelect: function () {
                            $grid[0].triggerToolbar(); 
                        }
                    });
                }
            }
        },
        { label: 'Nombre', name: 'CollaboratorFirstName', editable: false, align: 'center', headerClasses: 'my-column', cellattr: function () { return 'class="my-column"'; } },
        { label: 'Apellidos', name: 'CollaboratorLastName', align: 'center' },
        { label: 'Cédula', name: 'CollaboratorDNICollaborator', align: 'center', sorttype: 'number' },
        { label: 'Operador', name: 'CollaboratorOperatorNumber', align: 'center', sorttype: "number" },
        { label: 'E-mail', name: 'CollaboratorEmail', align: 'center', formatter: 'email' }
    ],
    onInitGrid: function () {
        $("<div class='alert-info-grid'>Sin registros</div>").insertAfter($(this).parent());
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
    ondblClickRow: function (rowId) {
        var rowData = jQuery(this).getRowData(rowId);
        var id = rowData['AnnotationId'];
        getAnnotationsDetails(id, "AnnotationByCollaboratorModal");
    },
    loadonce: true,
    shrinkToFit: true,
    altRows: true,
    pager: '#jqGridPager',
    rowNum: 20,
    rowList: [10, 20, 30, 50],
    viewrecords: true,
    rownumbers: true,
    sortable: true,
    autowidth: true,
    autoresizeOnLoad: true,
    autoresizeOnResize: true,
    height: '100%',
    width: '100%'
});

$grid.jqGrid('filterToolbar', {
    searchOnEnter: false,
    defaultSearch: "cn", 
    stringResult: true,
    ignoreCase: true
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
                $("#jqGrid").jqGrid('setGridParam', {
                }).trigger('resize');
            }
        });
    }
});