using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Attend
{
    public class AttendRepository : DbMapper, IAttendRepository
    {
        public List<Attend> GetAll()
        {
            return ExecuteList<Attend>("select_all_attend").ToList();
        }
        public int GetByCollaboratorId(int collaboratorid)
        {
            return ExecuteSql("select_attend_by_collaboratorid", new List<DbParameter>() {
                new DbParameter("param_collaboratorid", ParameterDirection.Input, collaboratorid) }
            );
        }
        public int GetById(int id)
        {
            return ExecuteSql("select_attend_by_id", new List<DbParameter>() {
                new DbParameter("param_attendid", ParameterDirection.Input, id) }
            );
        }
        public int Delete(int attendanceid)
        {
            return ExecuteSql("delete_attend_by_id", new List<DbParameter>() {
                new DbParameter("param_attendanceid", ParameterDirection.Input, attendanceid) }
            );
        }
        public int Insert(Attend attendance)
        {
            var id = ExecuteSqlMapObject("insert_attend", attendance);
            return (int)id;
        }
        public void Update(Attend attendance)
        {
            ExecuteSqlMapObject("update_attend", attendance);
        }//public Attend SearchAssistancePro(int collaboratorid, DateTime dateTime)
         //{
         //    var attends = ExecuteList<Attend>("select_all_attend").ToList();
         //    var attend = attends.Where(item => { return item.collaboratorid == collaboratorid && item.checkin.Date == dateTime.GetValueOrDefault().Date; }).FirstOrDefault();
         //    return attend;
         //}

        //public Attend SearchByCollaboratorIdDate(int collaboratorId, DateTime dateTime)
        //{
        //    return ExecuteSingle<Attend>("Select_Schedule_By_CollaboratorAttend_Id_Date", new List<DbParameter>()
        //        {
        //            new DbParameter("param_collaborator", ParameterDirection.Input, collaboratorId),
        //            new DbParameter("param_datetime", ParameterDirection.Input, dateTime)
        //        }
        //    );
        //}

        //public List<Attend> GetInformationAttendDatesRange(DateTime startDate, DateTime beginDate)
        //{
        //    return ExecuteListWithParameters<Attend>("Select_Attendance_By_Date_Range", new List<DbParameter>()
        //    {
        //        new DbParameter("param_startDate", ParameterDirection.Input, startDate),
        //        new DbParameter("param_endDate", ParameterDirection.Input, beginDate)
        //    }
        //    ).ToList();
        //}

        //public List<Attend> GetAttendByCollaboratorId(int collaboratorId)
        //{
        //    return ExecuteListWithParameters<Attend>("Select_Attend_Info_All",
        //    new List<DbParameter>() { new DbParameter("p_collaboratorid", ParameterDirection.Input, collaboratorId) }).ToList();
        //}

        //public List<Attend> GetAttendByCollaboratorIdFilterDate(int collaboratorId, DateTime? init, DateTime? final)
        //{
        //    var result = ExecuteListWithParameters<Attend>("Select_Attend_Info_All",
        //        new List<DbParameter>() { new DbParameter("p_collaboratorid", ParameterDirection.Input, collaboratorId) }).ToList();

        //    if (init == null && final == null)
        //    {
        //        return result;
        //    }

        //    if (init == null || final == null)
        //    {
        //        throw new ArgumentException("Ambas fechas deben estar definidas o ser nulas.");
        //    }

        //    if (init == final)
        //    {
        //        result = result.Where(x => x.checkin.Date == init.Value.Date).OrderBy(x => x.checkin).ToList();
        //    }
        //    else if (init < final) 
        //    {
        //        result = result.Where(x => x.checkin.Date >= init && x.checkin.Date <= final).OrderBy(x => x.checkin).ToList();
        //    }
        //    else
        //    {
        //        throw new ArgumentException("La fecha de inicio debe ser menor que la fecha final.");
        //    }

        //    return result;
        //}
    }
}
