using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Schedule.ScheduleDaily
{
    public class ScheduleDailyRepository : DbMapper, IScheduleDailyRepository
    {
        public List<ScheduleDaily> GetByScheduleId(int scheduleId)
        {
            return ExecuteListWithParameters<ScheduleDaily>("select_schedule_daily_by_scheduleid",
            new List<DbParameter>() { new DbParameter("param_scheduleid", ParameterDirection.Input, scheduleId) }).ToList();
        }

        public List<ScheduleDaily> GetByCollaboratorId(int collaboratorId)
        {
            return ExecuteListWithParameters<ScheduleDaily>("select_schedule_daily_by_scheduleid",
            new List<DbParameter>() { new DbParameter("param_collaboratorid", ParameterDirection.Input, collaboratorId) }).ToList();
        }
    }
}
