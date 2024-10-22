namespace PsiCollaborator.Data.Schedule
{
    public interface IScheduleCheckInStatus
    {
        int checkinstatusid { get; set; }
        string description { get; set; }
        string labelcheckinstatus { get; set; }
        string typecheckinstatus { get; set; }
    }
}