function GetWorkingDay(workingDayId) {
    if (!workingDayId) {
        Extension = new ScheduleDailyExtension([], null);
        return;
    }

    let formData = new FormData();
    formData.append("workingDayId", workingDayId);

    $.ajax({
        type: "POST",
        url: '/Schedule/GetWorkingDayData',
        data: formData,
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result.success) {
                let workingDay = new WorkingDay(
                    result.data.WorkingDayId,
                    result.data.MaxDays,
                    result.data.MaxHours,
                    Time.fromJsonDate(result.data.StartTime),
                    Time.fromJsonDate(result.data.EndTime)
                );
                Extension = new ScheduleDailyExtension([], workingDay);
            } else {
                console.error('Error en el servidor:', result.message);
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', status, error);
        }
    });
}

let form = document.getElementById("FormCreateSchedule");
form.addEventListener("submit", async (e) => {
    e.preventDefault();
    let name = document.querySelector('#Name').value;
    let workingDayId = document.querySelector('#WorkingDayId').value;
    let schedule = new Schedule(0, name, workingDayId, Extension.ScheduleDailys);

    if (Extension.ScheduleDailys.length === 0) {
        new Messi("Debe agregar al menos un día al horario.", { title: 'Error', titleClass: 'anim error', modal: true }).show();
        return;
    }
    SaveSchedule(schedule);
});

function SaveSchedule(schedule) {
    let scheduleTemp = schedule.ScheduleDailys.map(item => {
        item.DayId = item.ScheduleDayId;
        return item;
    });
    schedule.ScheduleDailys = scheduleTemp;
    $.ajax({
        url: "/Schedule/Save",
        type: 'POST',
        dataType: 'json',
        data: {
            schedule: JSON.stringify(schedule)
        },
        success: (data) => {
            if (data.success) {
                new Messi(data.message || "El horario se creo con éxito.", {
                    title: 'Horarios Creado con éxito',
                    titleClass: 'anim success',
                    modal: true
                }).show();

                setTimeout(() => {
                    window.location.href = "/Schedule/Index";
                }, 2000);
            } else {
                new Messi(data.message || "Hubo un error al crear el horario.", {
                    title: 'Error',
                    titleClass: 'anim error',
                    modal: true
                }).show();
            }
        },
        error: () => {
            new Messi("Error en la solicitud.", {
                title: 'Error',
                titleClass: 'anim error',
                modal: true
            }).show();
        }
    });
}
let Extension = new ScheduleDailyExtension([], null);