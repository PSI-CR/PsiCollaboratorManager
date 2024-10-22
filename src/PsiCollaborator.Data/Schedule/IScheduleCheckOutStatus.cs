namespace PsiCollaborator.Data.Schedule
{
    public interface IScheduleCheckOutStatus
    {
        int checkoutstatusid { get; set; }
        string description { get; set; }
        string labelcheckoutstatus { get; set; }
        string typecheckoutstatus { get; set; }
    }
}