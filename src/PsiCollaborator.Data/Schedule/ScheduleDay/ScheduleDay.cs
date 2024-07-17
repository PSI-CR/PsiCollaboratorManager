using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Schedule.ScheduleDay
{
    public class ScheduleDay : IScheduleDay
    {
        public int ScheduleDayId { get; set; }
        public string Name { get; set; }
    }
}
