using System;
using System.Collections.Generic;

namespace PsiCollaborator.Data.Attend
{
    public interface IAttendRepository
    {
        int Delete(int attendanceid);
        List<Attend> GetAll();
        int GetByCollaboratorId(int collaboratorid);
        int GetById(int id);
        int Insert(Attend attend);
        void Update(Attend attendance);
        List<Attend> GetAttendByCollaboratorId(int collaboratorId);
        List<Attend> GetInformationAttendDatesRange(DateTime startDate, DateTime beginDate);
        Attend SearchByCollaboratorIdDate(int collaboratorId, DateTime dateTime);
        List<Attend> GetInformationAttendDatesRangeByCollaborator(int collaboratorId, DateTime startDate, DateTime endDate);
    }
}