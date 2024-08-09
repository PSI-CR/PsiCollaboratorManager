using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Schedule
{
    public class Schedule : ISchedule
    {
        public int ScheduleId { get; set; }
        public string Name { get; set; }
        public int WorkingdayId { get; set; }
        public string[] ScheduleDailys { get; set; }
    }
}
