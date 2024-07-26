
namespace PsiCollaborator.Data.Schedule
{
    public class ScheduleData : IScheduleData
    {
        public int ScheduleId { get; set; }
        public string Name { get; set; }
        public string WorkingDayName { get; set; }
        public string WorkingDayDescription { get; set; }
        public bool Assigned { get; set; }
    }
}
