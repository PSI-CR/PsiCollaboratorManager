$("#BirthdayNextWeekGrid").jqGrid({
    url: '/Home/GetBirthdayNextWeek',
    datatype: "json",
    colModel: [
        { label: 'Id', name: 'CollaboratorId', editable: false, key: true, hidden: true },
        { label: 'Nombre', name: 'FirstName', editable: false, align: 'center' },
        { label: 'Apellidos', name: 'LastName', editable: false, align: 'center' },
        { label: 'Nacimiento', name: 'DateOfBirth', formatter: 'date', align: 'center', formatoptions: { newformat: 'd/m/Y' },
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
    ],
    pager: true,
    shrinkToFit: true,
    autowidth: true,
    autoresizeOnLoad: true,
    autoresizeOnResize: true,
    height: '100%',
    rowNum: 10,
    rowList: [10, 20, 30],
    viewrecords: true,
    caption: "Cumpleaños de la proxima semana ",
    ondblClickRow: function (rowId) {
        var rowData = jQuery(this).getRowData(rowId);
        var id = rowData['CollaboratorId'];
        GetCollaboratorDetails(id, "DetailsModal");
    },
    loadComplete: () => {
        resizeGridNw();
    }
});
$(window).on("resize", function () {
    resizeGridNw();
});
function resizeGridNw() {
    $("#BirthdayNextWeekGrid").jqGrid("setGridWidth", $("#BirthdayNextWeekGrid").closest(".ui-jqgrid").parent().width());
}