$grid = $("#jqGrid").jqGrid({
    url: '/Annotation/GetCollaborators',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { label: 'Id', name: 'CollaboratorId', editable: false, key: true, hidden: true, align: 'center' },
        { label: 'Nombre', name: 'FirstName', editable: false, align: 'center', headerClasses: 'my-column', cellattr: function () { return 'class="my-column"'; } },
        { label: 'Apellidos', name: 'LastName', align: 'center' },
        { label: 'Operador', name: 'OperatorNumber', align: 'center' }
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
    rowNum: 30,
    rowList: [10, 20, 30, 50],
    viewrecords: true,
    rownumbers: true,
    sortable: true,
    autowidth: true,
    autoresizeOnLoad: true,
    autoresizeOnResize: true,
    height: '100%',
    width: '100%',
    multiselect: true,
    onSelectRow: function (a, b, c) {
        if (this.p.selarrrow.length === currids.length) {
            $('#cb_' + $.jgrid.jqID(this.p.id), this.grid.hDiv)[this.p.useProp ? 'prop' : 'attr']("checked", true);
        }
    },
    gridComplete: function () {
        currids = $(this).jqGrid('getDataIDs');
    }
});
$(window).on("resize", function () {
    $("#jqGrid").jqGrid("setGridWidth", $("#jqGrid").closest(".ui-jqgrid").parent().width());
});
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
    }
);
$('#jqGrid').navButtonAdd('#jqGridPager',
    {
        buttonicon: "ui-icon-plusthick",
        title: "Nueva Anotación",
        caption: "Nueva Anotación",
        position: "last",
        onClickButton: getSelectedRows
    }
);
function getSelectedRows() {
    var grid = $("#jqGrid");
    var rowKey = grid.getGridParam("selrow");

    if (!rowKey) {
        new Messi('Favor seleccione un colaborador para agregar una anotación.', {
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
    else {
        var selectedIDs = grid.getGridParam("selarrrow");
        var lista = [];
        for (var i = 0; i < selectedIDs.length; i++) {
            var id = $("#jqGrid").jqGrid('getCell', selectedIDs[i], 'CollaboratorId');
            var namet = $("#jqGrid").jqGrid('getCell', selectedIDs[i], 'FirstName');
            var lastName = $("#jqGrid").jqGrid('getCell', selectedIDs[i], 'LastName');
            lista.push(id);
        }
        var dictJson = JSON.stringify(lista);
        location.href = '/Annotation/CreateAnnotation?collaboratorsDictJson=' + encodeURIComponent(dictJson);
    }
}
function resizeJqGridWidth(grid_id, div_id, width) {
    $(window).bind('resize', function () {
        $('#' + grid_id).setGridWidth(width, true);
        $('#' + grid_id).setGridWidth($('#' + div_id).width(), true);
    }).trigger('resize');
}