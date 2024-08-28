using PsiCollaborator.Data.Collaborator;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PsiCollaborator.Data.Schedule
{
    public class ScheduleRepository : DbMapper, IScheduleRepository
    {
        public List<ScheduleData> GetAll()
        {
            return ExecuteList<ScheduleData>("select_schedule_all").ToList();
        }
        public List<ScheduleBasic> GetAllBasic()
        {
            return ExecuteList<ScheduleBasic>("select_schedule_all_basic").ToList();
        }
        public void DismissSchedule(int collaboratorId)
        {
            ExecuteSql("Dismiss_schedule", new List<DbParameter>() {
                new DbParameter("param_collaboratorid", ParameterDirection.Input, collaboratorId) }
            );
        }
        public void AssignSchedule(int scheduleid, int collaboratorId)
        {
            ExecuteSql("Assign_schedule", new List<DbParameter>() {
                new DbParameter("param_scheduleid", ParameterDirection.Input, scheduleid),
                new DbParameter("param_collaboratorid", ParameterDirection.Input, collaboratorId)}
            );
        }

        public List<ScheduleData> GetAllScheduleWorking() 
        {
            return ExecuteList<ScheduleData>("select_data_schedule_workingday").ToList();
        }

        public List<ScheduleData> GetAllDays()
        {
            return ExecuteList<ScheduleData>("select_all_schedule_days").ToList();
        }

        public void Save(Schedule schedule)
        {
            ExecuteSqlMapObject("save_schedule", schedule);    
        }

        public Schedule GetById(int scheduleid)
        {
            var result = ExecuteSingle<Schedule>("select_schedule_full_by_id", new List<DbParameter>() {
                new DbParameter("param_scheduleid", ParameterDirection.Input, scheduleid)
            });
            return result;
        }
    }
}
