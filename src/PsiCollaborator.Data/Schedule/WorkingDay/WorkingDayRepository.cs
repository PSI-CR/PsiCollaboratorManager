using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PsiCollaborator.Data.Schedule.WorkingDay
{
    public class WorkingDayRepository : DbMapper, IWorkingDayRepository
    {
        public int Delete(int WorkingDayId)
        {
            return ExecuteSql("delete_working_day", new List<DbParameter>() { new DbParameter("param_working_day_id", ParameterDirection.Input, WorkingDayId) });
        }

        public void Insert(IWorkingDay workingDay)
        {
            ExecuteSqlMapObject("insert_working_day", workingDay);
        }

        public List<WorkingDay> GetAll()
        {
            return ExecuteList<WorkingDay>("select_working_day_all").ToList();
        }

        public WorkingDay GetById(int workingDayId)
        {
            return ExecuteSingle<WorkingDay>("select_all_working_day_by_id", new List<DbParameter>(){ new DbParameter("param_workingdayid", ParameterDirection.Input, workingDayId)});
        }
    }
}
