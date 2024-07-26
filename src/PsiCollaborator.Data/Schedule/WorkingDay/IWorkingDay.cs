using System;

namespace PsiCollaborator.Data.Schedule.WorkingDay
{
    public interface IWorkingDay
    {
        int WorkingDayId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int MaxDays { get; set; }
        int MaxHours { get; set; }
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        bool Accumulative { get; set; }
        bool Assigned { get; set; }       
        DateTime RecordTime { get; set; }

    }
} 