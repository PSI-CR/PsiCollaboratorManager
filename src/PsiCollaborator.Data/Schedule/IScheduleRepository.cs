using System.Collections.Generic;

namespace PsiCollaborator.Data.Schedule
{
    public interface IScheduleRepository
    {
        void AssignSchedule(int scheduleid, int collaboratorId);
        void DismissSchedule(int collaboratorId);
        List<ScheduleData> GetAll();
        List<ScheduleBasic> GetAllBasic();
    }
}