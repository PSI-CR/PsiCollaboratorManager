using System.Collections.Generic;

namespace PsiCollaborator.Data.Schedule.ScheduleDay
{
    public interface IScheduleDayRepository
    {
        List<ScheduleDay> GetAll();
    }
}