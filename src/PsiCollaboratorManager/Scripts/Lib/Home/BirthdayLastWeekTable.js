$("#BirthdayLastWeekGrid").jqGrid({
    url: '/Home/GetBirthdayLastWeek',
    datatype: "json",
    colModel: [
        { label: 'Id', name: 'CollaboratorId', editable: false, key: true, hidden: true },
        { label: 'Nombre', name: 'FirstName', editable: false, align: 'center' },
        { label: 'Apellidos', name: 'LastName', editable: false, align: 'center' },
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
        }
    ],
    loadonce:true,
    pager: true,
    shrinkToFit: true,
    autowidth: true,
    autoresizeOnLoad: true,
    autoresizeOnResize: true,
    sortable: true,
    altRows: true,
    height: '100%',
    rowNum: 10,
    rowList: [10, 20, 30],
    viewrecords: true,
    caption: "Cumpleaños de la semana anterior",
    ondblClickRow: function (rowId) {
        var rowData = jQuery(this).getRowData(rowId);
        var id = rowData['CollaboratorId'];
        GetCollaboratorDetails(id, "DetailsModal");
    },
    loadComplete: () => {
        resizeGridLw();
    }
});
$(window).on("resize", function () {
    resizeGridLw();
});
function resizeGridLw() {
    $("#BirthdayLastWeekGrid").jqGrid("setGridWidth", $("#BirthdayLastWeekGrid").closest(".ui-jqgrid").parent().width());
}