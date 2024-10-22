using PsiCollaborator.Data.Collaborator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Schedule.ScheduleDay
{
    public class ScheduleDaysByScheduleRepository : DbMapper, IScheduleDaysByScheduleRepository
    {
        public List<ScheduleDaysBySchedule> GetAll()
        {
            return ExecuteList<ScheduleDaysBySchedule>("get_all_schedule_daysbyschedule").ToList();
        }
    }
}
