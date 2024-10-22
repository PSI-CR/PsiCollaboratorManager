using System;

namespace PsiCollaborator.Data.Schedule.ScheduleDaily
{
    public class ScheduleDaily : IScheduleDaily
    {
        public int ScheduleDailyId { get; set; }
        public int DayId { get; set; }
        public string DayName { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
