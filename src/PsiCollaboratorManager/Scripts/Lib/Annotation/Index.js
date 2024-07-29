function getAnnotationsDetails(annotationId, modalId) {
    $.ajax({
        url: '/Annotation/GetAnnotationDetails?annotationId=' + annotationId,
        type: 'GET',
        dataType: 'json',
        success: function (annotation) {
            OpenModal(modalId);
            console.log(annotation.Date);
            if (annotation.CollaboratorPicture) {
                $('#' + modalId + '  .modalHeaderPhoto').attr('src', 'data:image/jpeg;base64,' + annotation.CollaboratorPicture);
            } else {
                $('#' + modalId + '  .modalHeaderPhoto').attr('src', '/Images/istockphoto-1300845620-612x612.jpg');
            }

            $('#' + modalId + ' .modalHeaderTitle').html(annotation.CollaboratorFirstName + " " + annotation.CollaboratorLastName);
            $('#' + modalId + ' #modal-OperatorNumber').html(annotation.CollaboratorOperatorNumber);
            $('#' + modalId + ' #modal-DNICollaborator').html(annotation.CollaboratorDNICollaborator);
            $('#' + modalId + ' #modal-AnnotationDate').html(displayDate(annotation.Date));
            $('#' + modalId + ' #modal-AnnotationType').html(annotation.AnnotationTypeName);
            $('#' + modalId + ' #modal-Annotation').html(annotation.Note);
        },
        error: function () {
            alert('Ha ocurrido un error al obtener los detalles del empleado.');
        }
    });
}

function Search() {
    var name = document.getElementById("_txtName").value;
    var lastName = document.getElementById("_txtLastName").value;
    var opNumber = document.getElementById("_txtOpNumber").value || -1;

    var startDate = formatDate(document.getElementById("_dtpStartDate").value) || "0001-01-01";
    var endDate = formatDate(document.getElementById("_dtpEndDate").value) || "9999-12-31";

    $.ajax({
        type: "POST",
        url: "/Annotation/SearchAnnotations",
        data: {
            name: name,
            lastName: lastName,
            opNumber: opNumber,
            startDate: startDate,
            endDate: endDate
        },
        success: function (response) {
            $("#jqGrid").jqGrid('clearGridData');
            $("#jqGrid").jqGrid('setGridParam', { data: response.rows }).trigger('reloadGrid');
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
    function formatDate(dateString) {
        if (dateString) {
            var date = new Date(dateString);
            var year = date.getFullYear();
            var month = String(date.getMonth() + 1).padStart(2, '0');
            var day = String(date.getDate()).padStart(2, '0');
            return year + "/" + month + "/" + day;
        }
        return null;
    }
}

function Refresh() {
    var name = "";
    var lastName = "";
    var opNumber = -1;
    var startDate = "0001-01-01";
    var endDate = "9999-12-31";

    $("#_txtName").val("");
    $("#_txtLastName").val("");
    $("#_txtOpNumber").val("");
    $("#_dtpStartDate").val("");
    $("#_dtpEndDate").val("");


    $.ajax({
        type: "POST",
        url: "/Annotation/SearchAnnotations",
        data: {
            name: name,
            lastName: lastName,
            opNumber: opNumber,
            startDate: startDate,
            endDate: endDate
        },
        success: function (response) {
            $("#jqGrid").jqGrid('clearGridData');
            $("#jqGrid").jqGrid('setGridParam', { data: response.rows }).trigger('reloadGrid');
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
}
function displayDate(dateString) {
    // Extract the timestamp from the string
    const timestamp = parseInt(dateString.match(/\d+/)[0]);

    // Create a new Date object from the timestamp
    const date = new Date(timestamp);

    // Get day, month, and year from the Date object
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are 0-indexed
    const year = date.getFullYear();

    // Format the date as dd/mm/yyyy
    return `${day}/${month}/${year}`;
}