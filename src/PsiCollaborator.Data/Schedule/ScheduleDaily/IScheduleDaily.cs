using System;

namespace PsiCollaborator.Data.Schedule.ScheduleDaily
{
    public interface IScheduleDaily
    {
        DateTime BeginTime { get; set; }
        DateTime EndTime { get; set; }
        int ScheduleDailyId { get; set; }
        string ScheduleDayName { get; set; }
    }
}