class ScheduleDaily {
    constructor(id, dayId, dayName, startTime, endTime) {
        this.ScheduleDailyId = id;
        this.ScheduleDayId = dayId;
        this.ScheduleDayName = dayName;
        this.BeginTime = startTime;
        this.EndTime = endTime;
        this.TotalTime = endTime.difference(startTime);
    }
}
