using System.Collections.Generic;

namespace PsiCollaborator.Data.Attend
{
    public interface IAttendRepository
    {
        int Delete(int attendanceid);
        List<Attend> GetAll();
        int GetByCollaboratorId(int collaboratorid);
        int GetById(int id);
        int Insert(Attend attendance);
        void Update(Attendance attendance);
        List<Attend> GetAttendByCollaboratorId(int collaboratorId);
    }
}