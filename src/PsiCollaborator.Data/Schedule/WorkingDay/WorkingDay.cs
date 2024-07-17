using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Schedule.WorkingDay
{
    public class WorkingDay : IWorkingDay
    {
        public int WorkingDayId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxDays { get; set; }
        public int MaxHours { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime RecordTime { get; set; }
        public bool Accumulative { get; set; }
        public bool Assigned { get; set; }
    }
}
