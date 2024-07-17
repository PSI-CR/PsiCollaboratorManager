using System.Collections.Generic;

namespace PsiCollaborator.Data.Schedule.ScheduleDaily
{
    public interface IScheduleDailyRepository
    {
        List<ScheduleDaily> GetByCollaboratorId(int collaboratorId);
        List<ScheduleDaily> GetByScheduleId(int scheduleId);
    }
}