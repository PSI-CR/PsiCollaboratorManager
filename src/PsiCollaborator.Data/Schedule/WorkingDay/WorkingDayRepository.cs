using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Schedule.WorkingDay
{
    public class WorkingDayRepository : DbMapper, IWorkingDayRepository
    {
        public int Delete(int WorkingDayId)
        {
            return ExecuteSql("delete_working_day", new List<DbParameter>() { new DbParameter("param_working_day_id", ParameterDirection.Input, WorkingDayId) });
        }

        public void Insert(WorkingDay workingDay)
        {
            ExecuteSqlMapObject("insert_working_day", workingDay);
        }

        public List<WorkingDay> GetAll()
        {
            return ExecuteList<WorkingDay>("select_working_day_all").ToList();
        }
    }
}
