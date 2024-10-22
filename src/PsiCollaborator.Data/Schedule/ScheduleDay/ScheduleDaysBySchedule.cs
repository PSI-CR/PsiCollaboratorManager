using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Schedule.ScheduleDay
{
    public class ScheduleDaysBySchedule : IScheduleDaysBySchedule
    {
        public int daybyscheduleid { get; set; }
        public int scheduleid { get; set; }
        public int scheduledailyid { get; set; }
    }
}
