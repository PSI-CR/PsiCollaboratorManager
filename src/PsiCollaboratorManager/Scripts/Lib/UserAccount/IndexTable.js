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
    height: '100%'

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
    { buttonicon: "ui-icon-pencil", title: "Edit", caption: "Editar", position: "last", onClickButton: editRow }
);

$('#jqGrid').navButtonAdd('#jqGridPager',
    { buttonicon: "ui-icon-plusthick", title: "Add", caption: "Agregar", position: "last", onClickButton: addRow }
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
    /*var grid = $("#jqGrid");*/
    var editUrl = '/UserAccount/Create';
    /*var fullUrl = window.location.protocol + '//' + window.location.host + editUrl;*/
    window.location.href = editUrl;
}

function displayDetails() {
    var grid = $("#jqGrid");
    var rowKey = grid.getGridParam("selrow");
    if (rowKey) {
        var key = $("#jqGrid").jqGrid('getCell', rowKey, 'UserAccountId');
        var editUrl = '/UserAccount/Details?userAccountId=' + key;
        /*var fullUrl = window.location.protocol + '//' + window.location.host + editUrl;*/
        window.location.href = editUrl;
    }
}

function editRow() {

    var grid = $("#jqGrid");
    var rowKey = grid.getGridParam("selrow");
    if (rowKey) {

        var key = $("#jqGrid").jqGrid('getCell', rowKey, 'UserAccountId');

        var editUrl = '/UserAccount/Edit?userAccountId=' + key;
        /*var fullUrl = window.location.protocol + '//' + window.location.host + editUrl;*/
        window.location.href = editUrl;

        //var editUrl = '@Url.Action("Edit", "LoginUserAccount", new { id = "_rowId_" })';
        //editUrl = editUrl.replace("_rowId_", key);
        //var fullUrl = window.location.protocol + '//' + window.location.host + editUrl;
        //window.location.href = fullUrl;
    }
    else {
        alert("No rows are selected");
    }
}