
function validateAndSubmitForm() {
    if (SaveValidateWorkingDay()) {
        document.getElementById("CreateWorkingDayForm").submit();
    }
}

function SaveValidateWorkingDay() {
    let maxStartHour = document.getElementById("StartTime").value;
    let maxEndHour = document.getElementById("EndTime").value;
    let maxTotalHours = document.getElementById("MaxHours").value;
    let maxTotalDays = document.getElementById("MaxDays").value;
    let timeTotalHours = "";

    let startTime = new Date("2000-01-01T" + maxStartHour);
    let endTime = new Date("2000-01-01T" + maxEndHour);

    if (startTime >= endTime) {
        showError("La hora de inicio debe ser menor que la hora de finalización.");
        return false;
    }

    timeTotalHours = maxTotalDays * 12;
    if (maxTotalHours > timeTotalHours) {
        showError("El máximo de horas, supero el limite de horas permitidas.");
        return false;
    }

    if (maxTotalDays >= 7 || maxTotalDays <= 0) {
        showError("El máximo de días, supero el limite de días permitidos.");
        return false;
    }
    return true;
}

function showError(message) {
    new Messi(message, { title: 'Error', titleClass: 'anim error', buttons: [{ id: 0, label: 'Cerrar', val: 'X' }] });
}




