using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models.Schedule
{
    public class ScheduleModel
    {
        public int ScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public int WorkingDayId { get; set; }
        public string WorkingDayName { get; set; }
        public string Description { get; set; }
        public string Assigned { get; set; }
        public int DayId { get; set; }
        public string WorkingDayStartTime { get; set; }
        public string WorkingDayEndTime { get; set; }
        public List<ScheduleRecords> ListRecordsTemp { get; set; }
    }

    public class ScheduleRecords
    {
        public int DayId { get; set; }
        public string Day { get; set; }
        public string HourBegin { get; set; }
        public string HourEnd { get; set; }
        public string Diferencia { get; set; }
    }
}