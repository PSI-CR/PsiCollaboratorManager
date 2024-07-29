function removeCollaborator(button) {
    $(button).closest('.collaboratorButton').remove();
}
function submitForm() {
    const isMessageTypeValid = validateMessageType();
    const isNoteValid = validateNote();
    const isMessageDateValid = validateMessageDate();

    if (isMessageTypeValid && isNoteValid && isMessageDateValid) {
        var formData = new FormData($('#CreateAnnotationForm')[0]);
        console.log(formData);
        $.ajax({
            url: '/Annotation/Create',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data != "") {
                    new Messi(data, { title: 'Error', titleClass: 'anim error', modal: true }).show();
                }
                else window.location.replace('../Annotation/Index');
            },
            error: function (error) {
                console.log('Error al enviar el formulario:', error);
            }
        });
    }
    else {
        return isMessageTypeValid && isNoteValid && isEmitterValid && isMessageDateValid && isFileValid;
    }
}

function validateMessageType() {
    const messageTypeDropdown = $("#MessageTypeDropDown");
    const selectedValue = messageTypeDropdown.val();

    if (selectedValue === "") {
        $("#MessageTypeError").show();
        messageTypeDropdown.addClass("is-invalid");
        return false;
    }
    else {
        $("#MessageTypeError").hide();
        messageTypeDropdown.removeClass("is-invalid");
        return true;
    }
}
function validateNote() {
    const noteTextArea = $("#NoteTextArea");
    const note = noteTextArea.val().trim();

    if (note === "") {
        $("#NoteError").show();
        noteTextArea.addClass("is-invalid");
        return false;
    }
    else {
        $("#NoteError").hide();
        noteTextArea.removeClass("is-invalid");
        return true;
    }
}
function validateMessageDate() {
    const messageDateInput = $("#MessageDateInput");
    const messageDate = messageDateInput.val().trim();

    if (messageDate === "") {
        $("#MessageDateError").show();
        messageDateInput.addClass("is-invalid");
        return false;
    }
    else {
        $("#MessageDateError").hide();
        messageDateInput.removeClass("is-invalid");
        return true;
    }
}
$(document).ready(function () {
    $("#MessageTypeDropDown").on("change", function () {
        validateMessageType();
    });
    $("#NoteTextArea").on("input", function () {
        validateNote();
    });
    $("#MessageDateInput").on("input", function () {
        validateMessageDate();
    });
    $("form.contact_form").on("submit", function () {
        const isMessageTypeValid = validateMessageType();
        const isNoteValid = validateNote();
        const isMessageDateValid = validateMessageDate();
        return isMessageTypeValid && isNoteValid && isMessageDateValid;
    });
});