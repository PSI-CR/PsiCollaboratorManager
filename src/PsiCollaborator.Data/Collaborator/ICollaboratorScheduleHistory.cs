using System;

namespace PsiCollaborator.Data.Collaborator
{
    public interface ICollaboratorScheduleHistory
    {
        bool Active { get; set; }
        DateTime AssignDate { get; set; }
        int CollaboratorId { get; set; }
        int CollaboratorScheduleHistoryId { get; set; }
        DateTime DismissDate { get; set; }
        int ScheduleId { get; set; }
    }
}