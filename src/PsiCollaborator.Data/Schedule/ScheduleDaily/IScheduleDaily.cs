using System;

namespace PsiCollaborator.Data.Schedule.ScheduleDaily
{
    public interface IScheduleDaily
    {
        int ScheduleDailyId { get; set; }
        int DayId { get; set; }
        string DayName { get; set; }
        DateTime BeginTime { get; set; }
        DateTime EndTime { get; set; }
        string ExtraTime { get; set; }
        string PendingTime { get; set; }
    }
}