$grid = $("#jqGrid").jqGrid({
    url: '/UserAccount/GetAll',
    mtype: 'GET',
    datatype: 'json',
    shrinkToFit: true,
    colModel: [
        { label: 'Id ', name: 'UserAccountId', editable: false, key: true, hidden: true },
        { label: 'Usuario', name: 'UserName'},
        { label: 'Nombre', name: 'FirstName'},
        { label: 'Apellidos', name: 'LastName' },
        { label: 'E-mail', name: 'Email' },
        { label: "Esta Activo", name: 'IsActive', align: 'center', formatter: "checkbox", formatoptions: { disabled: true } },
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
    ondblClickRow: function (rowId) {
        displayDetails()
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
    gridComplete: initGrid,
    height: '100%',
    width:'100%'
});
$(window).on("resize", function () {
    resizeGrid();
});
function resizeGrid() {
    $("#jqGrid").jqGrid("setGridWidth", $("#jqGrid").closest(".ui-jqgrid").parent().width());
}

$('#jqGrid').navGrid('#jqGridPager', { edit: false, add: false, del: false, search: true, refresh: true, view: false, position: "left", cloneToTop: true }, { multipleSearch: true, multipleGroup: true });

$('#jqGrid').navButtonAdd('#jqGridPager', { buttonicon: "ui-icon-calculator", title: "Column chooser", caption: "Columnas", position: "last",
    onClickButton: function () {
        jQuery("#jqGrid").jqGrid('columnChooser', {
            done: function (numColumn) {
                $("#jqGrid").jqGrid('setGridParam', { }).trigger('resize');
            }
        });
    }
});
$('#jqGrid').navButtonAdd('#jqGridPager',
    { buttonicon: "ui-icon-pencil", title: "Edit", caption: "Editar", position: "last", onClickButton: editRow }
);
$('#jqGrid').navButtonAdd('#jqGridPager',
    { buttonicon: "ui-icon-plusthick", title: "Add", caption: "Agregar", position: "last", onClickButton: addRow }
);
function initGrid() {
    $(".jqgrow", "#jqGrid").contextMenu('contextMenu', {
        bindings: {
            'edit': function (t) { editRow(); },
            'add': function (t) { addRow(); },
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
function addRow() {
    var editUrl = '/UserAccount/Create';
    window.location.href = editUrl;
}
function displayDetails() {
    var grid = $("#jqGrid");
    var rowKey = grid.getGridParam("selrow");
    if (rowKey) {
        var key = $("#jqGrid").jqGrid('getCell', rowKey, 'UserAccountId');
        var editUrl = '/UserAccount/Details?userAccountId=' + key;
        window.location.href = editUrl;
    }
}
function editRow() {

    var grid = $("#jqGrid");
    var rowKey = grid.getGridParam("selrow");
    if (rowKey) {
        var key = $("#jqGrid").jqGrid('getCell', rowKey, 'UserAccountId');
        var editUrl = '/UserAccount/Edit?userAccountId=' + key;
        window.location.href = editUrl;
    }
    else {
        alert("No hay una fila seleccionada");
    }
}