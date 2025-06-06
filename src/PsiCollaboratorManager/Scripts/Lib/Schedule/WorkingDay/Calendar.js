$(document).ready(function () {
    let isEditingEvent = false;
    let currentEvent = null;

    $('#calendar').fullCalendar({
        locale: 'es',
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },

        events: '/Calendar/GetEvents',

        selectable: true,
        selectHelper: true,

        select: function (start, end) {
            isEditingEvent = false;
            currentEvent = null;

            $('#eventModal').modal('show');
            $('#deleteEventBtn').hide();
            $('#editEventBtn').hide();

            let defaultStartHour = "08:00";
            let defaultEndHour = "09:00";
            let startFormatted = moment(start).format('YYYY-MM-DD') + `T${defaultStartHour}`;
            let endFormatted = moment(start).format('YYYY-MM-DD') + `T${defaultEndHour}`;

            $('#eventStart').val(startFormatted);
            $('#eventEnd').val(endFormatted);
            $('#eventTitle').val('');
            $('#eventColor').val('#ff0000');
            $('#modalCollaboratorsSelect').val(null).selectpicker('refresh');
        },

        eventClick: function (event) {
            isEditingEvent = true;
            currentEvent = event;

            $('#eventModal').modal('show');
            $('#deleteEventBtn').show();
            $('#editEventBtn').show();

            $('#eventTitle').val(event.title);
            $('#eventStart').val(event.start._i);
            $('#eventEnd').val(event.end._i);
            $('#eventColor').val(event.color || '#ff0000');
            $('#modalCollaboratorsSelect').val(event.collaboratorIds).selectpicker('refresh');
        }
    });

    // Cargar colaboradores
    $.ajax({
        url: '/Collaborator/GetAllActive',
        type: 'GET',
        success: function (data) {
            const collaborators = data.rows;
            const selectElements = $('#collaboratorsSelect, #modalCollaboratorsSelect');

            selectElements.empty();
            collaborators.forEach(collaborator => {
                selectElements.append(
                    $('<option>').val(collaborator.CollaboratorId).text(collaborator.FirstName + " " + collaborator.LastName)
                );
            });

            selectElements.selectpicker('refresh');
        },
        error: function () {
            alert('Error al cargar los colaboradores.');
        }
    });

    // Guardar evento
    $('#saveEventBtn').off('click').on('click', function () {
        let title = $('#eventTitle').val();
        let start = $('#eventStart').val();
        let end = $('#eventEnd').val() || null;
        let color = $('#eventColor').val();
        let collaboratorId = $('#modalCollaboratorsSelect').val();

        if (!title || !start) {
            alert('Por favor completa el título y las fechas.');
            return;
        }

        if (!collaboratorId || collaboratorId.length === 0) {
            alert('Debes seleccionar al menos un colaborador.');
            return;
        }

        const eventData = {
            Title: title,
            Description: '',
            StartDate: start,
            EndDate: end,
            IsAllDay: false,
            Color: color,
            CollaboratorId: collaboratorId
        };

        $.ajax({
            url: '/Calendar/SaveEvent',
            type: 'POST',
            data: JSON.stringify(eventData),
            contentType: 'application/json',
            success: function (response) {
                if (response.success) {
                    $('#calendar').fullCalendar('refetchEvents');
                    $('#eventModal').modal('hide');
                } else {
                    alert('Error: ' + response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
                alert('Ocurrió un error al guardar el evento.');
            }
        });
    });

    // Eliminar evento
    $('#deleteEventBtn').off('click').on('click', function () {
        if (!currentEvent || !currentEvent.id) {
            alert('Evento no válido para eliminar.');
            return;
        }

        if (!confirm('¿Estás seguro que deseas eliminar este evento?')) {
            return;
        }

        $.ajax({
            url: '/Calendar/Delete',
            type: 'POST',
            data: JSON.stringify({ id: currentEvent.id }),
            contentType: 'application/json',
            success: function (response) {
                if (response.success) {
                    $('#calendar').fullCalendar('refetchEvents');
                    $('#eventModal').modal('hide');
                } else {
                    alert('Error al eliminar: ' + response.message);
                }
            },
            error: function () {
                alert('Ocurrió un error al eliminar el evento.');
            }
        });
    });

    //Editar evento
    $('#editEventBtn').off('click').on('click', function () {
        let title = $('#eventTitle').val();
        let start = $('#eventStart').val();
        let end = $('#eventEnd').val() || null;
        let color = $('#eventColor').val();
        let collaboratorId = $('#modalCollaboratorsSelect').val();

        if (!title || !start) {
            alert('Por favor completa el título y las fechas.');
            return;
        }

        if (!collaboratorId || collaboratorId.length === 0) {
            alert('Debes seleccionar al menos un colaborador.');
            return;
        }

        const eventData = {
            Event_Id: currentEvent.id,
            Title: title,
            Description: '',
            StartDate: start,
            EndDate: end,
            IsAllDay: false,
            Color: color,
            CollaboratorId: collaboratorId
        };

        $.ajax({
            url: '/Calendar/UpdateEvent',
            type: 'POST',
            data: JSON.stringify(eventData),
            contentType: 'application/json',
            success: function (response) {
                if (response.success) {
                    $('#calendar').fullCalendar('refetchEvents');
                    $('#eventModal').modal('hide');
                } else {
                    alert('Error: ' + response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
                alert('Ocurrió un error al actualizar el evento.');
            }
        });
    });
});
