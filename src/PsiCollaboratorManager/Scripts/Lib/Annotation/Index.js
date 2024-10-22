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
    var startDate = formatDate(document.getElementById("_dtpStartDate").value) || "0001-01-01";
    var endDate = formatDate(document.getElementById("_dtpEndDate").value) || "9999-12-31";

    $.ajax({
        type: "POST",
        url: "/Annotation/SearchAnnotations",
        data: {        
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
    var startDate = "0001-01-01";
    var endDate = "9999-12-31";

    $("#_dtpStartDate").val("");
    $("#_dtpEndDate").val("");


    $.ajax({
        type: "POST",
        url: "/Annotation/SearchAnnotations",
        data: {
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
function displayDate(dateString)
{
    const timestamp = parseInt(dateString.match(/\d+/)[0]);
    const date = new Date(timestamp);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
}