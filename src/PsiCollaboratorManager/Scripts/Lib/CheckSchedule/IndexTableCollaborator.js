//$('#SectionHeaderTitle0').text('Lista de Colaboradores');
//$(document).ready(function () {
//    $("#jqGrid").jqGrid({
//        url: '/Schedule/GetCollaborator',
//        mtype: 'GET',
//        datatype: 'json',
//        colModel: [
//            { label: 'Id', name: 'CollaboratorId', key: true, hidden: true },
//            { label: 'Nombre', name: 'FirstName', align: 'center' },
//            { label: 'Apellidos', name: 'LastName', align: 'center' },
//            { label: 'Número Operador', name: 'OperatorNumber', align: 'center' }
//        ],
//        loadonce: true,
//        shrinkToFit: true,
//        altRows: true,
//        pager: '#jqGridPager',
//        rowNum: 30,
//        rowList: [10, 20, 30, 50],
//        viewrecords: true,
//        rownumbers: true,
//        sortable: true,
//        autowidth: true,
//        height: '100%',
//        width: '100%',
//        search: true,
//        onInitGrid: function () {
//            $("<div class='alert-info-grid'>Sin registros</div>").insertAfter($(this).parent());
//        },
//        loadComplete: function () {
//            var $this = $(this), p = $this.jqGrid("getGridParam");
//            if (p.reccount === 0) {
//                $this.hide();
//                $this.parent().siblings(".alert-info-grid").show();
//            } else {
//                $this.show();
//                $this.parent().siblings(".alert-info-grid").hide();
//            }
//        },
//        resizeStop: function (newWidth, index) {
//            var colModel = $(this).jqGrid('getGridParam', 'colModel');
//            var column = colModel[index];

//            console.log(newWidth);
//            if (column.width < column.minWidth) {
//                $(this).jqGrid('setGridParam', {
//                    colModel: colModel
//                }).trigger('resize');
//            }
//        },

//         ondblClickRow: function (rowId) {
//            var rowData = jQuery(this).getRowData(rowId);
//            var collaboratorId = rowData['CollaboratorId'];

//            $.ajax({
//                url: '/Schedule/GetAssistanceByCollaborator/',
//                type: 'GET',
//                data: { collaboratorId: collaboratorId },
//                dataType: 'json',
//                success: function (response) {
//                    $("#jqGrid2").jqGrid('clearGridData');
//                    var collaboratorName = 'Sin registros';
//                    var collaboratorPicture = '/Images/DefaultCollaborator.jpg';

//                    console.log(response);

//                    if (response.success && response.data) {
//                        collaboratorName = response.data.Firstname + ' ' + response.data.Lastname;
//                        collaboratorPicture = response.data.Picture;

//                        if (response.data.AttendModels && response.data.AttendModels.length > 0) {
//                            response.data.AttendModels.forEach(function (item) {
//                                item.IsOpenCheckIn = item.IsOpenCheckIn ? 'Sí' : 'No';
//                            });

//                            $("#jqGrid2").jqGrid('setGridParam', { data: response.data.AttendModels });
//                            $("#jqGrid2").trigger('reloadGrid');
//                        }
//                    }

//                    $('#SectionHeaderTitle').text(collaboratorName);
//                    $('#SectionHeaderPicture').attr('src', collaboratorPicture);
//                },
//                error: function () {
//                    alert('Ocurrió un error al intentar obtener los datos del colaborador.');
//                }
//            });
//        }
//    });

//    $("#jqGrid").jqGrid('filterToolbar', {
//        searchOperators: false,
//        searchOnEnter: false,
//        defaultSearch: "cn"
//    });

//});


$('#SectionHeaderTitle0').text('Lista de Colaboradores');
$(document).ready(function () {
    $("#jqGrid").jqGrid({
        url: '/Schedule/GetCollaborator',
        mtype: 'GET',
        datatype: 'json',
        colModel: [
            { label: 'Id', name: 'CollaboratorId', key: true, hidden: true },
            { label: 'Nombre', name: 'FirstName', align: 'center' },
            { label: 'Apellidos', name: 'LastName', align: 'center' },
            { label: 'Número Operador', name: 'OperatorNumber', align: 'center' }
        ],
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
        height: '100%',
        width: '100%',
        search: true,
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

        // Doble clic en una fila para seleccionar colaborador y cargar sus datos
        ondblClickRow: function (rowId) {
            var rowData = jQuery(this).getRowData(rowId);
            var collaboratorId = rowData['CollaboratorId'];  // Obtiene el CollaboratorId

            console.log('Colaborador seleccionado ID:', collaboratorId); // Verificar ID

            // Realiza la solicitud AJAX al servidor
            $.ajax({
                url: '/Schedule/GetAssistanceByCollaborator/',  // URL correcta
                type: 'GET',
                data: { collaboratorId: collaboratorId },  // Pasa el CollaboratorId al servidor
                dataType: 'json',
                success: function (response) {
                    // Limpia los datos actuales en jqGrid2 antes de cargar nuevos
                    $("#jqGrid2").jqGrid('clearGridData');
                    var collaboratorName = 'Sin registros';
                    var collaboratorPicture = '/Images/DefaultCollaborator.jpg';

                    console.log('Respuesta del servidor:', response);  // Verificar respuesta del servidor

                    if (response.success && response.data) {
                        // Si la respuesta es exitosa, extrae el nombre y la imagen del colaborador
                        collaboratorName = response.data.Firstname + ' ' + response.data.Lastname;
                        collaboratorPicture = response.data.Picture;

                        // Verifica y carga los registros de asistencia en jqGrid2
                        if (response.data.AttendModels && response.data.AttendModels.length > 0) {
                            response.data.AttendModels.forEach(function (item) {
                                item.IsOpenCheckIn = item.IsOpenCheckIn ? 'Sí' : 'No';  // Convierte booleano a texto
                            });

                            // Establece los datos en jqGrid2 y recarga la tabla
                            $("#jqGrid2").jqGrid('setGridParam', { data: response.data.AttendModels });
                            $("#jqGrid2").trigger('reloadGrid');
                        }
                    }

                    // Actualiza el nombre y la imagen en el encabezado
                    $('#SectionHeaderTitle').text(collaboratorName);
                    $('#SectionHeaderPicture').attr('src', collaboratorPicture);
                },
                error: function () {
                    alert('Ocurrió un error al intentar obtener los datos del colaborador.');
                }
            });
        }
    });

    // Configura la barra de filtro en jqGrid
    $("#jqGrid").jqGrid('filterToolbar', {
        searchOperators: false,
        searchOnEnter: false,
        defaultSearch: "cn"
    });

});