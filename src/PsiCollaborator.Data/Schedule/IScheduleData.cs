namespace PsiCollaborator.Data.Schedule
{
    public interface IScheduleData
    {
        int ScheduleId { get; set; }
        string Name { get; set; }
        string WorkingDayName { get; set; }
        string WorkingDayDescription { get; set; }
        bool Assigned { get; set; }
    }
}