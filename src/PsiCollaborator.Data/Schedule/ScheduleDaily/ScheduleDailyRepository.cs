using System.Collections.Generic;
using System.Data;
using System.Linq;

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
            return ExecuteListWithParameters<ScheduleDaily>("select_scheduledaily_by_collaboratorid",
            new List<DbParameter>() { new DbParameter("param_collaboratorid", ParameterDirection.Input, collaboratorId) }).ToList();
        }

        public List<ScheduleDaily> GetScheduleDailyByCollaboratorId(int collaboratorId)
        {
            return ExecuteListWithParameters<ScheduleDaily>("select_schedule_details_by_collaborator",
            new List<DbParameter>() { new DbParameter("param_collaboratorid", ParameterDirection.Input, collaboratorId) }).ToList();
        }
    }
}
