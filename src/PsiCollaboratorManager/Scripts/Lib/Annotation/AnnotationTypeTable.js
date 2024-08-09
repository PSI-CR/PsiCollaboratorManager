$grid = $("#jqGrid").jqGrid({
    url: '/AnnotationType/GetAll',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { label: 'Id', name: 'AnnotationTypeId', key: true, hidden: true},
        { label: 'Nombre', name: 'TypeName', align: 'center', headerClasses: 'my-column', cellattr: function () { return 'class="my-column"'; } },
        { label: 'Porcentaje', name: 'Percentage', align: 'center', sorttype: 'number' },
        { label: "Visible", name: 'VisibleToCollaborator', align: 'center', formatter: "checkbox", formatoptions: { disabled: true } },
        { label: "Valor en %", name: 'ValueInScore', align: 'center', formatter: "checkbox", formatoptions: { disabled: true } },
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
    loadonce: true,
    shrinkToFit: true,
    autowidth: true,
    autoresizeOnLoad: true,
    autoresizeOnResize: true,
    height: '100%',
    width: '100%',
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
function resizeGrid(){
    $("#jqGrid").jqGrid("setGridWidth", $("#jqGrid").closest(".ui-jqgrid").parent().width());
}
$('#jqGrid').navGrid('#jqGridPager',
    { edit: false, add: false, del: false, search: true, refresh: true, view: false, position: "left", cloneToTop: true }, { multipleSearch: true, multipleGroup: true }
);
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
$('#jqGrid').navButtonAdd('#jqGridPager', { buttonicon: "ui-icon-plusthick", title: "Add", caption: "Agregar", position: "last", onClickButton: addRow });
function addRow() {
    window.location.href = "/AnnotationType/Create";
}
function editRow() {

    var grid = $("#jqGrid");
    var rowKey = grid.getGridParam("selrow");
    if (rowKey) {

        var id = $("#jqGrid").jqGrid('getCell', rowKey, 'AnnotationTypeId');
        window.location.href = "/AnnotationType/Edit?annotationTypeId=" + id;
    }
    else {
        new Messi('Favor seleccione un tipo para editar', {
            title: 'Seleccione Tipo',
            titleClass: 'anim warning',
            modal: true
        });
    }
}