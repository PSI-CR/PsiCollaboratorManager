using PsiCollaborator.Data.Collaborator;
using System.Collections.Generic;

namespace PsiCollaborator.Data.Schedule
{
    public interface IScheduleRepository
    {
        void AssignSchedule(int scheduleid, int collaboratorId);
        void DismissSchedule(int collaboratorId);
        List<ScheduleData> GetAll();
        List<ScheduleBasic> GetAllBasic();
        List<ScheduleData> GetAllScheduleWorking();
        void Save(Schedule scheduleData);
        Schedule GetById(int scheduleid);
        List<ScheduleCheckInStatus> GetAllScheduleCheckInStatus();
        List<ScheduleCheckOutStatus> GetAllScheduleCheckOutStatus();
    }
}