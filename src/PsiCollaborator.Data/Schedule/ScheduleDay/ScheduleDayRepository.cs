using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Schedule.ScheduleDay
{
    public class ScheduleDayRepository : DbMapper, IScheduleDayRepository
    {
        public List<ScheduleDay> GetAll()
        {
            return ExecuteList<ScheduleDay>("select_all_days").ToList();
        }
    }
}
