using System.Collections.Generic;

namespace PsiCollaborator.Data.Schedule.WorkingDay
{
    public interface IWorkingDayRepository
    {
        int Delete(int WorkingDayId);
        List<WorkingDay> GetAll();
        void Insert(IWorkingDay workingDay);
    }
}