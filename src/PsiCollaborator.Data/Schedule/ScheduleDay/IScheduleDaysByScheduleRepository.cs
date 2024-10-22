using System.Collections.Generic;

namespace PsiCollaborator.Data.Schedule.ScheduleDay
{
    public interface IScheduleDaysByScheduleRepository
    {
        List<ScheduleDaysBySchedule> GetAll();
    }
}
