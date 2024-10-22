using PsiCollaborator.Data.UserAccount;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;

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
            var id = ExecuteSqlMapObject("insert_attend", attendance, "AttendanceId","LabelCheckInStatus", "LabelCheckOutStatus");
            return (int)id;
        }

        public void Update(Attend attendance)
        {
            ExecuteSqlMapObject("update_attend", attendance, "IpAddress", "PhysicalAddressEquipment");
        }

        public List<Attend> GetAttendByCollaboratorId(int collaboratorId)
        {
            return ExecuteListWithParameters<Attend>("select_collaborator_attendance",
            new List<DbParameter>() { new DbParameter("param_colaboratorId", ParameterDirection.Input, collaboratorId) }).ToList();
        }

        public List<Attend> GetInformationAttendDatesRange(DateTime startDate, DateTime beginDate)
        {
            return ExecuteListWithParameters<Attend>("Select_Attendance_By_Date_Range", new List<DbParameter>()
            {
                new DbParameter("param_startDate", ParameterDirection.Input, startDate),
                new DbParameter("param_endDate", ParameterDirection.Input, beginDate)
            }
            ).ToList();
        }

        public List<Attend> GetInformationAttendDatesRangeByCollaborator(int collaboratorID, DateTime startDate, DateTime beginDate)
        {
            return ExecuteListWithParameters<Attend>("get_attendance_by_collaborator_and_dates", new List<DbParameter>()
            {
                new DbParameter("param_collaboratorid", ParameterDirection.Input, collaboratorID),
                new DbParameter("param_start_date", ParameterDirection.Input, startDate),
                new DbParameter("param_end_date", ParameterDirection.Input, beginDate)
            }
            ).ToList();
        }

        public Attend SearchByCollaboratorIdDate(int collaboratorId, DateTime dateTime)
        {
            return ExecuteSingle<Attend>("Select_Schedule_By_CollaboratorAttend_Id_Date", new List<DbParameter>()
                {
                    new DbParameter("param_collaborator", ParameterDirection.Input, collaboratorId),
                    new DbParameter("param_datetime", ParameterDirection.Input, dateTime)
                }
            );
        }
    }
}
