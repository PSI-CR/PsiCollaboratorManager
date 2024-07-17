using System;

namespace PsiCollaborator.Data.Schedule.WorkingDay
{
    public interface IWorkingDay
    {
        bool Accumulative { get; set; }
        bool Assigned { get; set; }
        string Description { get; set; }
        DateTime EndTime { get; set; }
        int MaxDays { get; set; }
        int MaxHours { get; set; }
        string Name { get; set; }
        DateTime RecordTime { get; set; }
        DateTime StartTime { get; set; }
        int WorkingDayId { get; set; }
    }
}