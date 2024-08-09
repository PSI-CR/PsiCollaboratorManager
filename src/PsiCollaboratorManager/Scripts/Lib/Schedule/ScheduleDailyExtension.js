class ScheduleDailyExtension {
    constructor(scheduleDailys, workingDay) {
        this.ScheduleDailys = scheduleDailys;
        this.WorkingDay = workingDay;
        this.currentId = scheduleDailys.length>0? Math.max.apply(Math, scheduleDailys.map(x => { return x.ScheduleDailyId })) + 1:0;    
        this.DisplayData();
    }

    Add() {
        let scheduleDaily = this.GetGuiData();
        let message = this.Validate(scheduleDaily);
        if (message == null ){
            this.ScheduleDailys.push(scheduleDaily);
            this.DisplayData();
            this.currentId++;
            console.log(this.ScheduleDailys);
        }
        else {         
            this.DisplayMessage(message);     
        }
    }

    Delete(scheduleDailyId) {
        let index = this.ScheduleDailys.findIndex(scheduleDaily => scheduleDaily.ScheduleDailyId === scheduleDailyId);
        if (index !== -1) {
            this.ScheduleDailys.splice(index, 1);
            this.DisplayData();
        }
        else
        {
            this.DisplayMessage('No se pudo eliminar');
        }
    }

    Validate(scheduleDaily) {
        let workingDay = this.WorkingDay;

        if (!workingDay) {
            return "Por favor seleccione una jornada.";
        }

        for (let i = 0; i < this.ScheduleDailys.length; i++) {
            if (this.ScheduleDailys[i].ScheduleDayId == scheduleDaily.ScheduleDayId) {
                return "El día seleccionado ya está registrado en el horario.<br>Por favor, elija un día diferente.";
            }
        }
     
        if (scheduleDaily.BeginTime >= scheduleDaily.EndTime) {
            return "La hora inicial no puede ser mayor o igual a la hora final.<br>Por favor, ajusta las horas correctamente.";
        }

        if (this.ScheduleDailys.length >= workingDay.MaxDays) {
            return "Se ha alcanzado el máximo de días permitidos para el horario.<br>Por favor, elimina algunos días antes de añadir más.";
        }
 
        if (scheduleDaily.BeginTime < workingDay.StartTime) {
            return "La hora inicial está fuera del rango permitido.<br>La hora inicial de la jornada laboral seleccionada es " + workingDay.StartTime + ".<br>Por favor, ajústala dentro del intervalo permitido.";
        }

        if (scheduleDaily.EndTime > workingDay.EndTime) {
            return "La hora final está fuera del rango permitido.<br>La hora final de la jornada laboral seleccionada es " + workingDay.EndTime + ".<br>Por favor, ajústala dentro del intervalo permitido.";
        }

        let totalExistingHours = this.TimeSum();
        let newTotalTimeHours = scheduleDaily.TotalTime;
        if ((totalExistingHours.sum(newTotalTimeHours)) > new Time(workingDay.MaxHours,0)) {
            return "Se ha alcanzado el máximo de horas semanales permitidas.<br>El total de horas supera el límite establecido de " + workingDay.MaxHours + " horas.<br>Por favor, ajusta el horario para no exceder este límite.";
        }
        return null;
    }

    TimeSum() {
        let totalSum = Time.fromMidnight();
        for (let i = 0; i < this.ScheduleDailys.length; i++)
        {
            totalSum = this.ScheduleDailys[i].TotalTime.sum(totalSum);      
        }
        return totalSum;
    }

    GetGuiData() {
        let id = this.currentId;
        let dayId = parseInt(document.querySelector('#DayId').value);
        let startTime = document.querySelector('#inputHourInicial').value;
        let endTime = document.querySelector('#inputHourFinaly').value;
        let day = document.querySelector('#DayId option:checked').text;

        return new ScheduleDaily(id, dayId, day, Time.fromString(startTime), Time.fromString(endTime));
    }

    DisplayData() {
        const tbody = document.getElementById('tableScheduleCreate').querySelector('tbody');
        if (!tbody) {
            return;
        }
        tbody.innerHTML = '';

        for (let i = 0; i < this.ScheduleDailys.length; i++)
        {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${this.ScheduleDailys[i].ScheduleDayName}</td>
                <td>${this.ScheduleDailys[i].BeginTime.toString()}</td>
                <td>${this.ScheduleDailys[i].EndTime.toString()}</td>
                <td>${this.ScheduleDailys[i].TotalTime.toString()}</td>
                <td>
                    <button type="button" class="btn btn-danger btn-sm" onclick="Extension.Delete(${this.ScheduleDailys[i].ScheduleDailyId})">Eliminar</button>
                </td>
            `;
            tbody.appendChild(row);
        }
        document.getElementById('totalSum').value = this.TimeSum();
    }

    DisplayMessage(message) {
        new Messi(message, { title: 'Error', titleClass: 'anim error', modal: true }).show();
    }
}