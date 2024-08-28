using System.Collections.Generic;
using System.Data;
using System.Linq;

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
        public void Update(Attendance attendance)
        {
            ExecuteSqlMapObject("update_attend", attendance);
        }

        public List<Attend> GetAttendByCollaboratorId(int collaboratorId)
        {
            return ExecuteListWithParameters<Attend>("select_collaborator_attendance",
            new List<DbParameter>() { new DbParameter("param_colaboratorId", ParameterDirection.Input, collaboratorId) }).ToList();
        }
    }
}
