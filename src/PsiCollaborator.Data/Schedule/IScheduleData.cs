namespace PsiCollaborator.Data.Schedule
{
    public interface IScheduleData
    {
        bool Assigned { get; set; }
        string Name { get; set; }
        int ScheduleId { get; set; }
        string WorkingDayDescription { get; set; }
        string WorkingDayName { get; set; }
    }
}