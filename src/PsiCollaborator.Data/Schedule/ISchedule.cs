namespace PsiCollaborator.Data.Schedule
{
    public interface ISchedule
    {
        string[] ScheduleDailys { get; set; }
        int ScheduleId { get; set; }
        string Name { get; set; }
        int WorkingdayId { get; set; }
    }
}