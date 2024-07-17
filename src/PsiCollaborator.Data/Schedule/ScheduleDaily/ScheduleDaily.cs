using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Schedule.ScheduleDaily
{
    public class ScheduleDaily : IScheduleDaily
    {
        public int ScheduleDailyId { get; set; }
        public string ScheduleDayName { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
