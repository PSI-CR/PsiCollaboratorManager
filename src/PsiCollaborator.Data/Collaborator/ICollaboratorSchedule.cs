using System;

namespace PsiCollaborator.Data.Collaborator
{
    public interface ICollaboratorSchedule
    {
        DateTime AssignDate { get; set; }
        int CollaboratorId { get; set; }
        int CollaboratorScheduleHistoryId { get; set; }
        string DNICollaborator { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        int OperatorNumber { get; set; }
        string ScheduleName { get; set; }
    }
}