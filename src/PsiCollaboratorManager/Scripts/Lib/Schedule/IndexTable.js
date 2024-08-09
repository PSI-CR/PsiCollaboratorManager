$(function () {
    $grid = $("#jqGrid").jqGrid({
        url: '/Schedule/GetAllSchedules',
        mtype: 'GET',
        datatype: 'json',
        shrinkToFit: false,
        colNames: ['Id', 'Nombre', 'Jornada', 'Descripcion', 'Asignado'],
        colModel: [
            { name: 'ScheduleId', index: 'Id', width: 36, hidden: true, align: 'center' },
            { name: 'Name', index: 'Nombre', editable: true, align: 'center', editrules: { required: true }, width: 160 },
            { name: 'WorkingDayName', index: 'Jornada', width: 160, editable: true, align: 'center', editrules: { required: true } },
            { name: 'WorkingDayDescription', index: 'Descripción', width: 461, editable: true, align: 'center', editrules: { required: true } },
            { name: 'Assigned', index: 'Asignado', width: 166, editable: true, align: 'center', editrules: { required: true } }
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
            var rowData = jQuery(this).getRowData(rowId);
            var id = rowData['ScheduleId'];
            window.location.href = '/Schedule/Edit/?scheduleId=' + id;
        },

        loadonce: true,
        shrinkToFit: true,
        altRows: true,
        pager: '#jqGridPager',
        rowNum: 10,
        rowList: [10, 20, 30, 50],
        viewrecords: true,
        rownumbers: true,
        sortable: true,
        autowidth: true,
        autoresizeOnLoad: true,
        autoresizeOnResize: true,
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
            buttonicon: "ui-icon-plusthick",
            title: "Agregar",
            caption: "Agregar",
            position: "last",
            onClickButton: addWorkinDay
        }
    );

    function addWorkinDay() {
        window.location.href = "/Schedule/Create";
    }

});